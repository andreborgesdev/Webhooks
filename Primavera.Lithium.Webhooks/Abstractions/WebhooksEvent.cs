using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Primavera.Hydrogen.ComponentModel.DataAnnotations;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    /// <summary>
    /// Defines the model for the WebhooksEvent table.
    /// </summary>
    public class WebhooksEvent : TableEntity
    {
        [Required]
        public string Event { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
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
