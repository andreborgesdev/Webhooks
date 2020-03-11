using Primavera.Hydrogen.Storage.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    /// <summary>
    /// Defines the model for the EventTriggeredDto.
    /// </summary>
    public class EventTriggeredDto
    {
        /// <summary>
        /// Gets or sets the Event.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the Event.
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the TriggerDate.
        /// </summary>
        public DateTime TriggerDate { get; set; }

        /// <summary>
        /// Gets or sets the Payload.
        /// </summary>
        public string PayloadType { get; set; }

        /// <summary>
        /// Gets or sets the Payload.
        /// </summary>
        public string Payload { get; set; }
    }
}
