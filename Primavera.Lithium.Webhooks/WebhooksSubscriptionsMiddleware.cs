using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Webhooks.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Primavera.Lithium.Webhooks.Application;

namespace Primavera.Lithium.Webhooks
{
    public class WebhooksSubscriptionsMiddleware
    {
        #region Private Fields

        private IMediator _mediator;
        private readonly RequestDelegate _next;
        private readonly ILogger<WebhooksMiddleware> _logger;

        #endregion

        public WebhooksSubscriptionsMiddleware(RequestDelegate next, ILogger<WebhooksMiddleware> logger, IMediator mediator)
        {
            _next = next;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var webhooksMethod = context.Request.Method;
            var subscriptionQuery = context.Request.Query["subscription"];

            if (webhooksMethod == "GET" && string.IsNullOrWhiteSpace(subscriptionQuery))
            {
                await this.GetWebhooksSubscriptions(context);
            }
            else if (webhooksMethod == "GET" && !string.IsNullOrWhiteSpace(subscriptionQuery))
            {
                await this.GetWebhooksEventsForSubscription(context);
            }

            if (webhooksMethod == "POST")
            {
                await this.SubscribeWebhooksEvent(context);
            }

            if (webhooksMethod == "PUT")
            {
                await this.UpdateWebhooksSubscription(context);
            }

            if (webhooksMethod == "DELETE")
            {
                await this.UnsubscribeWebhooksEvent(context);
            }
        }

        public async Task<bool> GetWebhooksSubscriptions(HttpContext context)
        {
            IEnumerable<WebhooksSubscription> webhooksEvents = await this._mediator.Send(new GetWebhooksSubscriptionsQuery());

            var response = JsonConvert.SerializeObject(webhooksEvents);

            await context.Response.WriteAsync(response);

            return true;
        }

        public async Task<bool> GetWebhooksEventsForSubscription(HttpContext context)
        {
            try
            {
                var productQuery = context.Request.Query["subscription"];

                IEnumerable<WebhooksSubscription> webhooksSubscriptions = await this._mediator.Send(new GetWebhooksEventsForSubscriptionQuery() { Subscription = productQuery });

                var response = JsonConvert.SerializeObject(webhooksSubscriptions);

                await context.Response.WriteAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<bool> GetWebhooksSubscriptionsByEvent(HttpContext context)
        {
            try
            {
                var productQuery = context.Request.Query["event"];

                IEnumerable<WebhooksSubscription> webhooksSubscriptions = await this._mediator.Send(new GetWebhooksEventsForSubscriptionQuery() { Subscription = productQuery });

                var response = JsonConvert.SerializeObject(webhooksSubscriptions);

                await context.Response.WriteAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<bool> SubscribeWebhooksEvent(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    SubscribeWebhooksEventDto webhooksSubscriptionDto = JsonConvert.DeserializeObject<SubscribeWebhooksEventDto>(requestBody);

                    var response = await this._mediator.Send(new SubscribeWebhooksEventCommand() { WebhooksSubscriptionDto = webhooksSubscriptionDto });

                    await context.Response.WriteAsync(response.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<bool> UpdateWebhooksSubscription(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    WebhooksSubscription webhooksSubscription = JsonConvert.DeserializeObject<WebhooksSubscription>(requestBody);

                    var response = await this._mediator.Send(new UpdateWebhooksSubscriptionCommand() { WebhooksSubscription = webhooksSubscription });

                    await context.Response.WriteAsync(response.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<bool> UnsubscribeWebhooksEvent(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    WebhooksSubscription webhooksSubscription = JsonConvert.DeserializeObject<WebhooksSubscription>(requestBody);

                    var response = await this._mediator.Send(new UnsubscribeWebhooksEventCommand() { Subscription = webhooksSubscription.Subscription, Product = webhooksSubscription.Product , WebhooksEvent = webhooksSubscription.Event });
                    
                    await context.Response.WriteAsync(response.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }
    }
}
