using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;

namespace Primavera.Lithium.Faturacao.WebApi.Managers
{
    /// <summary>
    /// Defines the interface of the Webhooks manager.
    /// </summary>
    public interface IWebhooksSubscriptionManager
    {
        /// <summary>
        /// Creates the webhooks event.
        /// </summary>
        /// <param name="webhooksSubscription">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task SubscribeWebhooksEvents(Faturacao.Models.WebhooksSubscriptionDto webhooksSubscription);

        /// <summary>
        /// Creates the webhooks event.
        /// </summary>
        /// <param name="webhooksSubscription">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task SubscribeWebhooksEvent(WebhooksSubscription webhooksSubscription);

        /// <summary>
        /// Creates the applications asynchronous.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        Task<IEnumerable<WebhooksSubscription>> GetWebhooksSubscriptions();

        /// <summary>
        /// Creates the applications asynchronous.
        /// </summary>
        /// <param name="subscription">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task<IEnumerable<WebhooksSubscription>> GetWebhooksEventsForSubscription(string subscription);

        /// <summary>
        /// Creates the webhooks event.
        /// </summary>
        /// <param name="subscription">The webhooks event.</param>
        /// <param name="webhooksEvent">The event subscriber.</param>
        /// <returns>Asynchronous task.</returns>
        Task UnsubscribeWebhooksEvent(string subscription, string webhooksEvent);
    }
}
