using System;
using System.IO;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Hydrogen.Storage.Azure.Tables;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;
using Primavera.Lithium.Faturacao.WebApi.BackgroundServices;
using Primavera.Lithium.Faturacao.WebApi.Configuration;
using Primavera.Lithium.Faturacao.WebApi.Controllers;
using Primavera.Lithium.Faturacao.WebApi.Managers;
using Primavera.Lithium.Webhooks;

namespace Primavera.Lithium.Faturacao.WebApi
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public partial class Startup
    {
        /// <inheritdoc />
        protected override void AddBackgroundServices(IServiceCollection services, HostConfiguration hostConfiguration)
        {
            services.AddWebhooksBackgroundServices();
        }

        /// <inheritdoc />
        protected override void AddAdditionalServices(IServiceCollection services, HostConfiguration hostConfiguration)
        {
            SmartGuard.NotNull(() => hostConfiguration, hostConfiguration);

            services.AddAzureTableStorage(
                (options) =>
                {
                    options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=lithiumhubstg;AccountKey=yvZ5vPvWfAkhY6VEEdB/vrRMTUtLavTli4dBJFi9uueF6VDl/6LKDIdqydfVTCTFTynYknOvdcmY2X/ofVAqgg==;EndpointSuffix=core.windows.net;";
                });

            ////services.AddAzureEventBus(
            ////    (options) =>
            ////    {
            ////        options.ConnectionString = "Endpoint=sb://tbx-eventbus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8cjm4NSJBpQvRZy/QlYxReESPp/tQC23h35X5hH6Dug=";
            ////        options.EventHandlerOptions = new AzureEventBusEventHandlerOptions(autoComplete: false, maxConcurrentCalls: 10);
            ////        options.RetryStrategy = new ExponentialBackoffRetryStrategy();
            ////    });

            this.SetupManagers(services);

            ////this.EventBusSubscription(services);

            this.BuildMediator(services);

            services.AddWebhooksServices(hostConfiguration.Information.HostTitle);

            ////services
            ////    .AddApplicationWebHooks(
            ////        (options) =>
            ////        {
            ////            options.Webhooks<InvoiceCreate>().Register();
            ////            options.Webhooks<InvoiceUpdate>().Register();
            ////        });
        }

        /// <inheritdoc />
        protected override void AdditionalAppConfigurations(IApplicationBuilder app, HostConfiguration hostConfiguration)
        {
            app.UseWebhooks();
        }

        /// <summary>
        /// Setups the managers.
        /// </summary>
        /// <param name="services">The services.</param>
        internal void SetupManagers(IServiceCollection services)
        {
            services.AddTransient<IWebhooksEventManager, WebhooksEventManager>();
            services.AddTransient<IWebhooksSubscriptionManager, WebhooksSubscriptionManager>();
        }

        /// <summary>
        /// Setups the managers.
        /// </summary>
        /// <param name="services">The services.</param>
        internal void EventBusSubscription(IServiceCollection services)
        {
            IEventBusEventHandler<WebhooksSubscription> messageEventHandler = new MessageEventHandler();

            services.BuildServiceProvider().GetRequiredService<IEventBus>().Subscribe("webhooks", messageEventHandler);
        }

        /// <summary>
        /// Setups the managers.
        /// </summary>
        /// <param name="services">The services.</param>
        internal void BuildMediator(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
