using System;
using System.Collections.Generic;
using System.Text;

namespace Primavera.Lithium.Faturacao.Models
{
    /// <inheritdoc/>
    public partial class WebhooksSubscription
    {
        /// <summary>
        /// Gets or sets asdasd.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <inheritdoc/>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.Properties = new Dictionary<string, object>();
        }
    }
}
