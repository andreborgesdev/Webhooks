using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Hydrogen.Storage.Azure.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class AddWebhooksServicesOptions
    {
        public AzureEventBusOptions EventBus { get; set; }

        public AzureTableStorageOptions TableStorage { get; set; }

        public string ServiceNamespace { get; set; }

        public string Product { get; set; }

        public IList<WebhooksEvent> WebHooks { get; set; }

        public AddWebhooksServicesOptions()
        {
            this.EventBus = new AzureEventBusOptions
            {
                EventHandlerOptions = new AzureEventBusEventHandlerOptions(autoComplete: false, maxConcurrentCalls: 10),
                RetryStrategy = new ExponentialBackoffRetryStrategy()
            };

            this.WebHooks = new List<WebhooksEvent>();
            this.TableStorage = new AzureTableStorageOptions();
        }

        public void RegisterWebhook(string webhookEvent, string description, string requiredScope)
        {
            this.WebHooks.Add(new WebhooksEvent()
            {
                Event = webhookEvent,
                Description= description,
                RequiredScope = requiredScope,
                Product = this.ServiceNamespace.Replace(" ", String.Empty)
        });
        }
    }
}
