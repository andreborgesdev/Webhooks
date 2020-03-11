using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Primavera.Lithium.Faturacao.Models;
using Primavera.Lithium.Faturacao.WebApi.CustomCode.Mappers;
using Primavera.Lithium.Faturacao.WebApi.Managers;

namespace Primavera.Lithium.Faturacao.WebApi.Controllers
{
    /// <summary>
    /// Extension of WebhooksController Auto generated class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>")]
    public partial class WebhooksController
    {
        #region Members

        private IWebhooksEventManager webhooksEventManager;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private IWebhooksEventManager WebhooksEventManager
        {
            get
            {
                if (this.webhooksEventManager == null)
                {
                    this.webhooksEventManager = (IWebhooksEventManager)this.HttpContext.RequestServices.GetService(typeof(IWebhooksEventManager));
                }

                return this.webhooksEventManager;
            }
        }

        #endregion

        #region Actions

        /// <inheritdoc />
        protected override async Task<IActionResult> GetWebhooksEventsCoreAsync()
        {
            var events = await this.WebhooksEventManager.GetWebhooksEvents().ConfigureAwait(false);

            return this.Ok(events);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> CreateWebhooksEventCoreAsync(WebhooksEvent webhooksEvent)
        {
            var response = await this.WebhooksEventManager.CreateWebhooksEvent(MapExtensions.ToWebhooksEvent(webhooksEvent)).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return this.NoContent();
            }

            return this.StatusCode(500);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetWebhooksEventsByProductCoreAsync(string product)
        {
            var events = await this.WebhooksEventManager.GetWebhooksEventsByProduct(product).ConfigureAwait(false);

            return this.Ok(events);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> DeleteWebhooksEventCoreAsync(string product, string webhooksEvent)
        {
            await this.WebhooksEventManager.DeleteWebhooksEvent(product, webhooksEvent).ConfigureAwait(false);

            return this.NoContent();
        }

        #endregion
    }
}
