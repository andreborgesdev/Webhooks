using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Primavera.Hydrogen;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;

namespace Primavera.Lithium.Faturacao.WebApi.Managers
{
    /// <summary>
    /// Provides the default implementation of the <see cref="IWebhooksEventManager"/>.
    /// </summary>
    public class WebhooksSubscriptionManager : IWebhooksSubscriptionManager
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<WebhooksSubscriptionManager> Logger { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhooksSubscriptionManager" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public WebhooksSubscriptionManager(IServiceProvider serviceProvider, ILogger<WebhooksSubscriptionManager> logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
            this.table = this.GetTableAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public async Task SubscribeWebhooksEvents(Faturacao.Models.WebhooksSubscriptionDto webhooksSubscription)
        {
            SmartGuard.NotNull(() => webhooksSubscription, webhooksSubscription);

            foreach (var eventSubscription in webhooksSubscription.Events)
            {
                WebhooksSubscription subscripition = new WebhooksSubscription()
                {
                    Event = eventSubscription,
                    Product = webhooksSubscription.Product,
                    Subscriber = webhooksSubscription.Subscriber,
                    NotificationEndpoint = webhooksSubscription.NotificationEndpoint,
                    Properties = webhooksSubscription.Properties
                };

                await this.SubscribeWebhooksEvent(subscripition).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task SubscribeWebhooksEvent(WebhooksSubscription webhooksSubscription)
        {
            SmartGuard.NotNull(() => webhooksSubscription, webhooksSubscription);

            webhooksSubscription.Key1 = webhooksSubscription.Subscriber;
            webhooksSubscription.Key2 = webhooksSubscription.Product + ":" + webhooksSubscription.Event;

            await this.table.InsertEntityAsync<WebhooksSubscription>(webhooksSubscription).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WebhooksSubscription>> GetWebhooksSubscriptions()
        {
            return await this.table.RetrieveEntitiesAsync<WebhooksSubscription>().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WebhooksSubscription>> GetWebhooksEventsForSubscription(string subscription)
        {
            return await this.table.RetrieveEntitiesAsync<WebhooksSubscription>(subscription).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task UnsubscribeWebhooksEvent(string subscription, string webhooksEvent)
        {
            WebhooksSubscription subscriptionToDelete = await this.table.RetrieveEntityAsync<WebhooksSubscription>(subscription, webhooksEvent).ConfigureAwait(false);

            await this.table.DeleteEntityAsync<WebhooksSubscription>(subscriptionToDelete).ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Interface of the Lock Service.
        /// </summary>
        private async Task<ITableReference> GetTableAsync()
        {
            ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
            ITableReference table = service.GetTable("WebhooksSubscription");

            return table;
        }

        #endregion
    }
}
