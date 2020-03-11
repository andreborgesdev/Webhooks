using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using Primavera.Hydrogen.Storage.Azure;

namespace Primavera.Lithium.Faturacao.WebApi.Attributes
{
    /// <summary>
    /// EntityJsonPropertyConverter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EntityJsonPropertyConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityJsonPropertyConverterAttribute"/> class.
        /// </summary>
        public EntityJsonPropertyConverterAttribute()
        {
        }
    }
}
