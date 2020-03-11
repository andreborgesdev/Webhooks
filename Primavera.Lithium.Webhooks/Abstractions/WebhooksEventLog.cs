using System;
using System.Collections.Generic;
using System.Text;
using Primavera.Hydrogen.Storage.Tables;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class WebhooksEventLog : ITableEntity
    {
        public string Event { get; set; }

        public string Product { get; set; }

        public string Subscription { get; set; }

        public bool Success { get; set; }

        public int Retries { get; set; }

        public DateTime NextRetry { get; set; }

        public string EventPayload { get; set; }

        /// <inheritdoc/>
        public string Key1 { get; set; }

        /// <inheritdoc/>
        public string Key2 { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; set; }
    }
}
