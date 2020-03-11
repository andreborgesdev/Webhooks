using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;


namespace Primavera.Lithium.Webhooks.Application
{
    public class GetWebhooksEventsForSubscriptionQuery : IRequest<IEnumerable<WebhooksSubscription>>
    {
        public string Subscription { get; set; }
    }
}
