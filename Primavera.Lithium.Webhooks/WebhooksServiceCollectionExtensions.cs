using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.EventBus.Azure.Entities;
using Primavera.Hydrogen.EventBus.Contracts;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Hydrogen.Storage.Azure.Tables;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;
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
            services.AddBackgroundServiceTimedWithWorker<RetryToSendWebhooksToSubscriptionsService, RetryToSendWebhooksToSubscriptionsWorker>();

            // Add QueuedBackgroundWorker and QueuedBackgroundService 
            services.AddTransient<SendWebhooksToSubscriptionsWorker>();
            services.AddSingleton<IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>, BackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>>();
            services.AddBackgroundServiceQueuedWithWorker<WebhooksQueuedBackgroundService, SendWebhooksToSubscriptionsWorker>();

            return services;
        }

        public static IServiceCollection AddWebhooksServices(this IServiceCollection services, Action<AddWebhooksServicesOptions> configureDelegate = null)
        {
            AddWebhooksServicesOptions webhooksOptions = new AddWebhooksServicesOptions();

            configureDelegate?.Invoke(webhooksOptions);

            services.AddAzureTableStorage(
                (options) =>
                {
                    options.ConnectionString = webhooksOptions.TableStorage.ConnectionString;
                });

            // Add Mediator service
            services.AddMediatR(Assembly.GetExecutingAssembly());

            var serviceNamespace = webhooksOptions.ServiceNamespace.Replace(" ", String.Empty).ToLower();
            var product = webhooksOptions.Product.Replace(" ", String.Empty);

            services.AddTransient<IWebhooksEventBusPublish, WebhooksEventBusPublish>();
            services.AddSingleton<WebhooksOptions>(s =>
            {
                return new WebhooksOptions(serviceNamespace, product);
            });

            // Add EventBus
            services.AddAzureEventBus(
                (options) =>
                {
                    options.ConnectionString = webhooksOptions.EventBus.ConnectionString;
                    options.EventHandlerOptions = new AzureEventBusEventHandlerOptions(autoComplete: false, maxConcurrentCalls: 10);
                    options.RetryStrategy = new ExponentialBackoffRetryStrategy();
                });

            IEventBusEventHandler<EventTriggeredDto> messageEventHandler = new MessageEventHandler(services.BuildServiceProvider());
            IEventBusEventFilters<EventTriggeredDto> messageFilters = new AzureEventBusEventFilters<EventTriggeredDto>();

            var serviceProvider = services.BuildServiceProvider();
            var eventbus = serviceProvider.GetRequiredService<IEventBusService>();

            messageFilters.Filters.Add("Service", serviceNamespace);

            eventbus.Subscribe("webhooks", messageEventHandler, messageFilters);

            foreach (var webhook in webhooksOptions.WebHooks)
            {
                serviceProvider.GetRequiredService<IMediator>().Send(new CreateWebhooksEventCommand() { WebhooksEvent = webhook }); 
            }

            return services;
        }
    }
}
