using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Primavera.Hydrogen.ComponentModel.DataAnnotations;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class RegisterWebhooksEventsOptions
    {
        [NotEmpty]
        public string Description { get; set; }

        [Required]
        public string Event { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string RequiredScope { get; set; }

        public RegisterWebhooksEventsOptions() { }
    }
}
