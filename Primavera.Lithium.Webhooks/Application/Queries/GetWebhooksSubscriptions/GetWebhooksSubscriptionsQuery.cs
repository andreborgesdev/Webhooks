using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class GetWebhooksSubscriptionsQuery : IRequest<IEnumerable<WebhooksSubscription>>
    {
    }
}
