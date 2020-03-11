using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Primavera.Hydrogen.EventBus;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;

namespace Primavera.Lithium.Faturacao.WebApi.Managers
{
    /// <summary>
    /// Defines the interface of the Webhooks manager.
    /// </summary>
    public interface IWebhooksEventManager
    {
        /// <summary>
        /// Creates the webhooks event.
        /// </summary>
        /// <param name="webhooksEvent">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task<BaseResponse> CreateWebhooksEvent(WebhooksEvent webhooksEvent);

        /// <summary>
        /// Creates the applications asynchronous.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        Task<IEnumerable<WebhooksEvent>> GetWebhooksEvents();

        /// <summary>
        /// Creates the applications asynchronous.
        /// </summary>
        /// <param name="product">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task<IEnumerable<WebhooksEvent>> GetWebhooksEventsByProduct(string product);

        /// <summary>
        /// Deletes the webhooks event.
        /// </summary>
        /// <param name="product">The webhooks publisher.</param>
        /// <param name="webhooksEvent">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task<BaseResponse> DeleteWebhooksEvent(string product, string webhooksEvent);

        /// <summary>
        /// Deletes the webhooks event.
        /// </summary>
        /// <param name="eventBusEvent">The webhooks event.</param>
        /// <returns>Asynchronous task.</returns>
        Task SendEventToSubscribers(IEventBusEvent<string> eventBusEvent);
    }
}
