using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Azure;
using Primavera.Hydrogen.EventBus.Azure.Entities;
using Primavera.Hydrogen.EventBus.Contracts;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.EventBus
{
    public class WebhooksEventBusPublish : IWebhooksEventBusPublish
    {
        private IServiceProvider ServiceProvider { get; }

        private IEventBusService EventBus
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IEventBusService>();
            }
        }
        private string ServiceNamespace
        {
            get
            {
                var options = this.ServiceProvider.GetRequiredService<WebhooksOptions>();

                return options.ServiceNamespace;
            }
        }

        private string Product
        {
            get
            {
                var options = this.ServiceProvider.GetRequiredService<WebhooksOptions>();

                return options.Product;
            }
        }

        public WebhooksEventBusPublish(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
        }

        public async Task Publish(EventTriggeredDto eventTriggeredDto, Dictionary<string, string> additionalProperties = null) 
        {
            IEventBusEvent<EventTriggeredDto> @event = new AzureEventBusEvent<EventTriggeredDto>()
            {
                Body = eventTriggeredDto
            };

            @event.Body.Product = Product;
            @event.Body.TriggerDate = DateTime.Now;

            @event.Properties.Add("Service", ServiceNamespace);

            this.EventBus.Publish(@event);
        }
    }
}
