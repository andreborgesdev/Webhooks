using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Primavera.Lithium.Faturacao.WebApi.CustomCode.Mappers
{
    /// <summary>
    /// Extensions methods to map objects.
    /// </summary>
    public static class MapExtensions
    {
        #region WebhooksEvents

        /// <summary>
        /// Connverts AppDto to App.
        /// </summary>
        /// <param name="webhooksEventDto">Application Dto.</param>
        /// <returns>An Instance of WebhooksEvent.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public static Abstractions.WebhooksEvent ToWebhooksEvent(this Faturacao.Models.WebhooksEvent webhooksEventDto)
        {
            Abstractions.WebhooksEvent webhooksEvent = new Abstractions.WebhooksEvent()
            {
                Timestamp = DateTime.Now,
                Event = webhooksEventDto.Event,
                Product = webhooksEventDto.Product,
                Description = webhooksEventDto.Description,
                RequiredScope = webhooksEventDto.RequiredScope,
            };

            return webhooksEvent;
        }

        #endregion
    }
}
