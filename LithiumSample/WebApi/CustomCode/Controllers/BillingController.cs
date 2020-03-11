using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Primavera.Hydrogen;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Lithium.Faturacao.Models;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.BackgroundServices;
using Primavera.Lithium.Webhooks.EventBus;

namespace Primavera.Lithium.Faturacao.WebApi.Controllers
{
    /// <summary>
    /// Extension of Faturacao Auto generated class.
    /// </summary>
    public partial class BillingController
    {
        private IEventBus EventBus
        {
            get
            {
                return (IEventBus)this.HttpContext.RequestServices.GetService(typeof(IEventBus));
            }
        }

        private IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker> WorkerQueue
        {
            get
            {
                return (IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>)this.HttpContext.RequestServices.GetService(typeof(IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>));
            }
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetInvoicesCoreAsync()
        {
            List<Invoice> invoices = new List<Invoice>()
            {
                new Invoice
                {
                    Id = default(Guid),
                    Amount = "500€"
                },
                new Invoice
                {
                    Id = default(Guid),
                    Amount = "1000€"
                },
                new Invoice
                {
                    Id = default(Guid),
                    Amount = "4123€"
                },
            };

            return this.Ok(invoices);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> CreateInvoiceCoreAsync(string amount)
        {
            SmartGuard.NotNull(() => amount, amount);

            ////this.WorkerQueue.Enqueue(
            ////        new SendWebhooksToSubscriptionsWorker(
            ////            this.HttpContext.RequestServices,
            ////            (ILogger<SendWebhooksToSubscriptionsWorker>)this.HttpContext.RequestServices.GetService(typeof(ILogger<SendWebhooksToSubscriptionsWorker>))));

            ////IEventBusEvent<WebhooksSubscription> @event = new AzureEventBusEvent<WebhooksSubscription>()
            ////{
            ////    Body = new WebhooksSubscription()
            ////    {
            ////        Event = "invoice_created",
            ////        Product = "Jasmin",
            ////    }
            ////};
            ////IEventBusEvent<EventTriggeredDto> @event = new AzureEventBusEvent<EventTriggeredDto>()
            ////{
            ////    Body = new EventTriggeredDto()
            ////    {
            ////        Product = "Jasmin",
            ////        Event = "bill_created",
            ////        TriggerDate = DateTime.Now,
            ////        PayloadType = "Type",
            ////        Payload = amount
            ////    }
            ////};

            ////@event.Properties.Add("Service", "primaveralithiumfaturacaoservice");

            ////@event.Body.Properties.Add("SubscriptionAlias", "10001-003");
            ////@event.Body.Properties.Add("CompanyTaxId", "123456789");

            ////this.EventBus.Publish(@event);

            ////this.WebHooks.Publish("invoice_created");

            EventTriggeredDto eventTriggeredDto = new EventTriggeredDto()
            {
                Product = "Jasmin",
                Event = "bill_created",
                TriggerDate = DateTime.Now,
                PayloadType = "Type",
                Payload = amount
            };

            IWebhooksEventBusPublish webhooksEventBusPublish = new WebhooksEventBusPublish(this.HttpContext.RequestServices);

            await webhooksEventBusPublish.Publish("primaveralithiumfaturacaoservice", eventTriggeredDto);

            return this.Ok();
        }
    }
}
