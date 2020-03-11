using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.EventBus.InMemory;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.EventBus
{
    public class WebhooksEventBusPublish : IWebhooksEventBusPublish
    {
        private IServiceProvider ServiceProvider { get; }

        private IEventBus EventBus
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IEventBus>();
            }
        }

        public WebhooksEventBusPublish(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
        }

        public async Task Publish(string serviceNamespace, EventTriggeredDto eventTriggeredDto, Dictionary<string, string> additionalProperties = null) 
        {
            IEventBusEvent<EventTriggeredDto> @event = new AzureEventBusEvent<EventTriggeredDto>()
            {
                Body = eventTriggeredDto
            };

            //IEventBusEvent<EventTriggeredDto> @event = new InMemoryEventBusEvent<EventTriggeredDto>()
            //{
            //    Body = eventTriggeredDto
            //};

            @event.Properties.Add("Service", serviceNamespace);

            ////@event.Body.Properties.Add("SubscriptionAlias", "10001-003");
            ////@event.Body.Properties.Add("CompanyTaxId", "123456789");

            this.EventBus.Publish(@event);
        }

        /// <summary>
        /// Gets the event bus service.
        /// </summary>
        /// <returns>The event bus service.</returns>
        private static IEventBus GetEventBusService()
        {
            return new InMemoryEventBus();
        }
    }
}
