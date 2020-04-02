using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.EventBus
{
    public interface IWebhooksEventBusPublish
    {
        Task Publish(EventTriggeredDto eventTriggeredDto, Dictionary<string, string> additionalProperties = null);
    }
}
