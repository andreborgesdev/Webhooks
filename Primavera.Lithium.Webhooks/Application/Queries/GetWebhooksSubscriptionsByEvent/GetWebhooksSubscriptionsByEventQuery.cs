using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Primavera.Lithium.Webhooks.Application
{
    public class GetWebhooksSubscriptionsByEventQuery : IRequest<IEnumerable<WebhooksSubscription>>
    {
        public string Product { get; set; }
        public string Event { get; set; }
    }
}
