using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Primavera.Hydrogen;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;

namespace Primavera.Lithium.Webhooks
{
    public static class RegisterWebhooksEventsExtensionMethod
    {
        public async static Task RegisterWebhooksEvents(this IServiceCollection services, RegisterWebhooksEventsOptions registerWebhooksEvents)
        {
            SmartGuard.NotNull(() => registerWebhooksEvents, registerWebhooksEvents);

            WebhooksEvent webhooksEvent = new WebhooksEvent()
            {
                Description = registerWebhooksEvents.Description,
                Event = registerWebhooksEvents.Event,
                Product = registerWebhooksEvents.Product,
                RequiredScope = registerWebhooksEvents.RequiredScope
            };

            await services.BuildServiceProvider().GetRequiredService<IMediator>().Send(new CreateWebhooksEventCommand() { WebhooksEvent = webhooksEvent });
        }
    }
}
