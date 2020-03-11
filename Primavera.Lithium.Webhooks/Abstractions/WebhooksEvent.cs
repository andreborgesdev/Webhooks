using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    /// <summary>
    /// Defines the model for the WebhooksEvent table.
    /// </summary>
    public class WebhooksEvent : TableEntity
    {
        public string Description { get; set; }

        public string Event { get; set; }

        public string Product { get; set; }

        public string RequiredScope { get; set; }

        public WebhooksEvent(string Product, string Event)
        {
            Key1 = Product;
            Key2 = Event;
            Timestamp = DateTime.Now;
        }

        public WebhooksEvent()
        {

        }
    }
}
