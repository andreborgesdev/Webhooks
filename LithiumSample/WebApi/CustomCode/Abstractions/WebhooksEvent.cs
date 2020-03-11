using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Faturacao.WebApi.Abstractions
{
    /// <summary>
    /// Defines the model for the WebhooksEvent table.
    /// </summary>
    public class WebhooksEvent : Faturacao.Models.WebhooksEvent, ITableEntity
    {
        /// <inheritdoc/>
        public string Key1 { get; set; }

        /// <inheritdoc/>
        public string Key2 { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; set; }
    }
}
