using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Primavera.Hydrogen.Rest.Routing;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Webhooks.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using System.Buffers;
using MediatR;
using Primavera.Lithium.Webhooks.Application;

namespace Primavera.Lithium.Webhooks
{
    public class WebhooksMiddleware
    {
        #region Private Fields

        private IMediator _mediator;
        private readonly RequestDelegate _next;
        private readonly ILogger<WebhooksMiddleware> _logger;

        #endregion

        public WebhooksMiddleware(RequestDelegate next, ILogger<WebhooksMiddleware> logger, IMediator mediator)
        {
            _next = next;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _mediator = context.RequestServices.GetRequiredService<IMediator>();

            var webhooksMethod = context.Request.Method;
            var productQuery = context.Request.Query["product"];

            if (webhooksMethod == "GET" && string.IsNullOrWhiteSpace(productQuery))
            {
                await this.GetWebhooksEvents(context);
            }
            else if (webhooksMethod == "GET" && !string.IsNullOrWhiteSpace(productQuery))
            {
                await this.GetWebhooksEventsByProduct(context);
            }

            if (webhooksMethod == "POST")
            {
                await this.CreateWebhooksEvent(context);
            }

            if (webhooksMethod == "PUT")
            {
                await this.UpdateWebhooksEvent(context);
            }

            if (webhooksMethod == "DELETE")
            {
                await this.DeleteWebhooksEvent(context);
            }
        }

        public async Task<bool> GetWebhooksEvents(HttpContext context)
        {
            IEnumerable<WebhooksEvent> webhooksEvents = await this._mediator.Send(new GetWebhooksEventsQuery());

            var response = JsonConvert.SerializeObject(webhooksEvents);

            await context.Response.WriteAsync(response);

            return true;
        }

        public async Task<bool> GetWebhooksEventsByProduct(HttpContext context)
        {
            try
            {
                var productQuery = context.Request.Query["product"];

                IEnumerable<WebhooksEvent> webhooksEvents = await this._mediator.Send(new GetWebhooksEventsByProductQuery() { Product = productQuery });

                var response = JsonConvert.SerializeObject(webhooksEvents);

                await context.Response.WriteAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<bool> CreateWebhooksEvent(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    WebhooksEvent webhooksEvent = JsonConvert.DeserializeObject<WebhooksEvent>(requestBody);

                    var response = await this._mediator.Send(new CreateWebhooksEventCommand() { WebhooksEvent = webhooksEvent });

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

        public async Task<bool> UpdateWebhooksEvent(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    WebhooksEvent webhooksEvent = JsonConvert.DeserializeObject<WebhooksEvent>(requestBody);

                    var response = await this._mediator.Send(new UpdateWebhooksEventCommand() { WebhooksEvent = webhooksEvent });

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

        public async Task<bool> DeleteWebhooksEvent(HttpContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var requestBody = reader.ReadToEndAsync().Result;

                    WebhooksEvent webhooksEvent = JsonConvert.DeserializeObject<WebhooksEvent>(requestBody);

                    var response = await this._mediator.Send(new DeleteWebhooksEventCommand() { Product = webhooksEvent.Product, WebhooksEvent = webhooksEvent.Event });
                    
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
