using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class SaveWebhooksEventLogCommand : IRequest<BaseResponse>
    {
        public string Event { get; set; }

        public string Product { get; set; }

        public string Subscription { get; set; }

        public EventTriggeredDto EventPayload { get; set; }
    }
}
