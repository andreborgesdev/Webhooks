using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Primavera.Hydrogen.Storage.Tables;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace Primavera.Lithium.Webhooks
{
    public static class WebhooksBuilderExtensions
    {
        #region Public Constants

        /// <summary>
        /// Defines the route for the following action: Create Invoice.
        /// </summary>
        public const string GetWebhooks = "/.webhooks";

        /// <summary>
        /// Defines the route for the following action: Get Invoices.
        /// </summary>
        public const string GetSubscriptions = "/.webhooks/subscriptions";

        #endregion

        public static IApplicationBuilder UseWebhooks(this IApplicationBuilder builder)
        {
            builder.Map(GetSubscriptions, b => b.UseMiddleware<WebhooksSubscriptionsMiddleware>());
            builder.Map(GetWebhooks, b => b.UseMiddleware<WebhooksMiddleware>());

            return builder;
        }
    }
}
