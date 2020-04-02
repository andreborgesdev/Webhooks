using Primavera.Hydrogen.Storage.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the Event.
        /// </summary>
        [Required]
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the TriggerDate.
        /// </summary>
        [Required]
        public DateTime TriggerDate { get; set; }

        /// <summary>
        /// Gets or sets the Payload.
        /// </summary>
        [Required]
        public string PayloadType { get; set; }

        /// <summary>
        /// Gets or sets the Payload.
        /// </summary>
        [Required]
        public string Payload { get; set; }
    }
}
