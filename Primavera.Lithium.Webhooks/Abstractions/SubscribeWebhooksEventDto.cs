using System;
using System.Collections.Generic;
using System.Text;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class SubscribeWebhooksEventDto
    {
        public virtual string[] Events { get; set; }

        public virtual string Subscription { get; set; }
        
        public virtual string Product { get; set; }
        
        public virtual string NotificationEndpoint { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public SubscribeWebhooksEventDto()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}
