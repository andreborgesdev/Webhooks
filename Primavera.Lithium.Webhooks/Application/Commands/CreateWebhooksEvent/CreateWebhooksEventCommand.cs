using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public class CreateWebhooksEventCommand : IRequest<BaseResponse>
    {
        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public WebhooksEvent WebhooksEvent { get; set; }
    }
}
