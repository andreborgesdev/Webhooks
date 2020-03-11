using System;
using System.Collections.Generic;
using Microsoft.Azure.Storage;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    /// <summary>
    /// Defines the model for the WebhooksSubscription table.
    /// </summary>
    public class WebhooksSubscription : TableEntity
    {
        public string Event { get; set; }

        public string NotificationEndpoint { get; set; }

        public string Product { get; set; }

        public string Subscription { get; set; }

        public string Properties { get; set; }

        public WebhooksSubscription(string Product, string Event, string Subscription)
        {
            Key1 = Product + ":" + Event;
            Key2 = Subscription;
            Timestamp = DateTime.Now;
        }

        public WebhooksSubscription()
        {

        }
    }
}
