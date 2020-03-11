using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Primavera.Lithium.Faturacao.WebApi.Attributes
{
    /// <summary>
    /// EntityJsonPropertyConverter class.
    /// </summary>
    public static class EntityJsonPropertyConverter
    {
        /// <summary>
        /// Extension of SubscriptionController Auto generated class.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="results">The result.</param>
        public static void Serialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> results)
        {
            entity.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EntityJsonPropertyConverterAttribute), false).Count() > 0)
                .ToList()
                .ForEach(x => results.Add(x.Name, new EntityProperty(JsonConvert.SerializeObject(x.GetValue(entity)))));
        }

        /// <summary>
        /// Extension of SubscriptionController Auto generated class.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="properties">The properties.</param>
        public static void Deserialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> properties)
        {
            entity.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EntityJsonPropertyConverterAttribute), false).Count() > 0)
                .ToList()
                .ForEach(x => x.SetValue(entity, JsonConvert.DeserializeObject(properties[x.Name].StringValue, x.PropertyType)));
        }
    }
}
