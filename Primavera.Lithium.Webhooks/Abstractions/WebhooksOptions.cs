using System;
using System.Collections.Generic;
using System.Text;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    public class WebhooksOptions
    {
        public string ServiceNamespace { get; set; }

        public string Product { get; set; }

        public WebhooksOptions(string serviceNamespace, string product)
        {
            this.ServiceNamespace = serviceNamespace;
            this.Product = product;
        }
    }
}
