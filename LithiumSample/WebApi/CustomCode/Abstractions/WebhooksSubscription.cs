using System;
using System.Collections.Generic;
using Microsoft.Azure.Storage;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Faturacao.WebApi.Attributes;

namespace Primavera.Lithium.Faturacao.WebApi.Abstractions
{
    /// <summary>
    /// Defines the model for the WebhooksSubscription table.
    /// </summary>
    public class WebhooksSubscription : Faturacao.Models.WebhooksSubscription, ITableEntity
    {
        /// <inheritdoc/>
        public string Key1 { get; set; }

        /// <inheritdoc/>
        public string Key2 { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; set; }
    }
}
