using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;

namespace Primavera.Lithium.Faturacao.WebApi.Application
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public class DeleteWebhooksEventCommand : IRequest<BaseResponse>
    {
        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public string WebhooksEvent { get; set; }
    }
}
