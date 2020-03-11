using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class SubscribeWebhooksEventCommand : IRequest<BaseResponse>
    {
        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public SubscribeWebhooksEventDto WebhooksSubscriptionDto { get; set; }
    }
}
