using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Primavera.Hydrogen;
using Primavera.Lithium.Faturacao.Models;
using Primavera.Lithium.Faturacao.WebApi.CustomCode.Mappers;
using Primavera.Lithium.Faturacao.WebApi.Managers;

namespace Primavera.Lithium.Faturacao.WebApi.Controllers
{
    /// <summary>
    /// Extension of SubscriptionController Auto generated class.
    /// </summary>
    public partial class SubscriptionController
    {
        #region Members

        private IWebhooksSubscriptionManager webhooksSubscriptionManager;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private IWebhooksSubscriptionManager WebhooksSubscriptionManager
        {
            get
            {
                if (this.webhooksSubscriptionManager == null)
                {
                    this.webhooksSubscriptionManager = (IWebhooksSubscriptionManager)this.HttpContext.RequestServices.GetService(typeof(IWebhooksSubscriptionManager));
                }

                return this.webhooksSubscriptionManager;
            }
        }

        #endregion

        #region Actions

        /// <inheritdoc />
        protected override async Task<IActionResult> SubscribeWebhooksEventCoreAsync(WebhooksSubscriptionDto webhooksSubscription)
        {
            SmartGuard.NotNull(() => webhooksSubscription, webhooksSubscription);

            await this.WebhooksSubscriptionManager.SubscribeWebhooksEvents(webhooksSubscription).ConfigureAwait(false);

            return this.NoContent();
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetWebhooksSubscriptionsCoreAsync()
        {
            var events = await this.WebhooksSubscriptionManager.GetWebhooksSubscriptions().ConfigureAwait(false);

            return this.Ok(events);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetWebhooksEventsForSubscriptionCoreAsync(string subscription)
        {
            var events = await this.WebhooksSubscriptionManager.GetWebhooksEventsForSubscription(subscription).ConfigureAwait(false);

            return this.Ok(events);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> UnsubscribeWebhooksEventCoreAsync(string webhooksEvent, string subscription)
        {
            await this.WebhooksSubscriptionManager.UnsubscribeWebhooksEvent(webhooksEvent, subscription).ConfigureAwait(false);

            return this.NoContent();
        }

        #endregion
    }
}
