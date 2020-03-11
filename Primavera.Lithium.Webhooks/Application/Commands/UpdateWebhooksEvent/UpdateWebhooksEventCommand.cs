using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class UpdateWebhooksEventCommand : IRequest<BaseResponse>
    {
        public WebhooksEvent WebhooksEvent { get; set; }
    }
}
