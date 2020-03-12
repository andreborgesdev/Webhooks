using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.EventBus.InMemory;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.BackgroundServices;
using Primavera.Lithium.Webhooks.EventBus;

namespace Primavera.Lithium.Webhooks
{
    public static class WebhooksServiceCollectionExtensions
    {
        public static IServiceCollection AddWebhooksBackgroundServices(this IServiceCollection services)
        {
            // Add TimedBackgroundService
            services.AddTransient<RetryToSendWebhooksToSubscriptionsWorker>();
            services.AddSingleton<IBackgroundWorkQueue<RetryToSendWebhooksToSubscriptionsWorker>, BackgroundWorkQueue<RetryToSendWebhooksToSubscriptionsWorker>>();
            //services.AddBackgroundServiceTimed<RetryToSendWebhooksToSubscriptionsService>();
            services.AddBackgroundServiceTimedWithWorker<RetryToSendWebhooksToSubscriptionsService, RetryToSendWebhooksToSubscriptionsWorker>();
            //services.AddBackgroundServiceQueuedWithWorker<RetryToSendWebhooksToSubscriptionsService, RetryToSendWebhooksToSubscriptionsWorker>();

            // Add QueuedBackgroundWorker and QueuedBackgroundService 
            services.AddTransient<SendWebhooksToSubscriptionsWorker>();
            services.AddSingleton<IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>, BackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>>();
            services.AddBackgroundServiceQueuedWithWorker<WebhooksQueuedBackgroundService, SendWebhooksToSubscriptionsWorker>();

            return services;
        }

        public static IServiceCollection AddWebhooksServices(this IServiceCollection services, string serviceNamespace)
        {
            // Add Mediator service
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IWebhooksEventBusPublish, WebhooksEventBusPublish>();

            //Add EventBus
            services.AddAzureEventBus(
                (options) =>
                {
                    options.ConnectionString = "Endpoint=sb://tbx-eventbus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8cjm4NSJBpQvRZy/QlYxReESPp/tQC23h35X5hH6Dug=";
                    options.EventHandlerOptions = new AzureEventBusEventHandlerOptions(autoComplete: false, maxConcurrentCalls: 10);
                    options.RetryStrategy = new ExponentialBackoffRetryStrategy();
                });

            // MessageEventHandler: IEventBusEventHandler<EventTriggeredDto>
            //services.AddSingleton<IEventBus, InMemoryEventBus>();
            //services.AddTransient<IEventBusEventHandler<EventTriggeredDto>, MessageEventHandler>();
            //services.AddTransient<IEventBusEventFilters<EventTriggeredDto>, InMemoryEventBusEventFilters<EventTriggeredDto>>();
            //services.AddTransient<IEventBusEventFilters<EventTriggeredDto>, AzureEventBusEventFilters<EventTriggeredDto>>();
           
            IEventBusEventHandler<EventTriggeredDto> messageEventHandler = new MessageEventHandler(services.BuildServiceProvider());
            IEventBusEventFilters<EventTriggeredDto> messageFilters = new AzureEventBusEventFilters<EventTriggeredDto>();
            //IEventBusEventFilters<EventTriggeredDto> messageFilters = new InMemoryEventBusEventFilters<EventTriggeredDto>();

            var serviceProvider = services.BuildServiceProvider();
            var eventbus = serviceProvider.GetRequiredService<IEventBus>();
            //var messageEventHandler = serviceProvider.GetRequiredService<IEventBusEventHandler<EventTriggeredDto>>();
            //var messageFilters = serviceProvider.GetRequiredService<IEventBusEventFilters<EventTriggeredDto>>();

            messageFilters.Filters.Add("Service", serviceNamespace.Replace(" ", String.Empty).ToLower());

            eventbus.Subscribe("webhooks", messageEventHandler, messageFilters);

            return services;
        }


        /// <summary>
        /// Gets the event bus service.
        /// </summary>
        /// <returns>The event bus service.</returns>
        //private static IEventBus GetEventBusService()
        //{
        //    return new InMemoryEventBus();
        //}
    }
}
