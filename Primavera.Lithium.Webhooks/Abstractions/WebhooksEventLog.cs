using System;
using System.Collections.Generic;
using System.Text;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class WebhooksEventLog : TableEntity
    {
        public string Event { get; set; }

        public string Product { get; set; }

        public string Subscription { get; set; }

        public string NotificationEndpoint { get; set; }

        public bool Success { get; set; }

        public int Retries { get; set; }

        public DateTime NextRetry { get; set; }

        public DateTime DeliveredOn { get; set; }

        public string EventPayload { get; set; }

        public WebhooksEventLog(string Product, string Event)
        {
            Key1 = Product + ":" + Event;
            Key2 = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }

        public WebhooksEventLog()
        {

        }
    }
}
