using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class UnsubscribeWebhooksEventCommand : IRequest<BaseResponse>
    {
        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public string Subscription { get; set; }
        
        public string Product { get; set; }

        public string WebhooksEvent { get; set; }
    }
}
