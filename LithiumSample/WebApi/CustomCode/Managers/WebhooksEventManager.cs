using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;
using Primavera.Lithium.Faturacao.WebApi.Application;

namespace Primavera.Lithium.Faturacao.WebApi.Managers
{
    /// <summary>
    /// Provides the default implementation of the <see cref="IWebhooksEventManager"/>.
    /// </summary>
    public class WebhooksEventManager : IWebhooksEventManager
    {
        #region Private Fields

        private IMediator mediator;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<WebhooksEventManager> Logger { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhooksEventManager" /> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">The logger.</param>
        public WebhooksEventManager(IServiceProvider serviceProvider, ILogger<WebhooksEventManager> logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
            this.mediator = this.ServiceProvider.GetRequiredService<IMediator>();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public async Task<BaseResponse> CreateWebhooksEvent(WebhooksEvent webhooksEvent)
        {
            SmartGuard.NotNull(() => webhooksEvent, webhooksEvent);
            
            return await this.mediator.Send(new CreateWebhooksEventCommand() { WebhooksEvent = webhooksEvent });
        }

        /// <inheritdoc />
        public async Task<BaseResponse> DeleteWebhooksEvent(string product, string webhooksEvent)
        {
            return await this.mediator.Send(new DeleteWebhooksEventCommand() { Product = product, WebhooksEvent = webhooksEvent });
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WebhooksEvent>> GetWebhooksEvents()
        {
            return await this.mediator.Send(new GetWebhooksEventsQuery());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WebhooksEvent>> GetWebhooksEventsByProduct(string product)
        {
            return await this.mediator.Send(new GetWebhooksEventsByProductQuery() { Product = product });
        }

        /// <inheritdoc />
        public async Task SendEventToSubscribers(IEventBusEvent<string> eventBusEvent)
        {
            SmartGuard.NotNull(() => eventBusEvent, eventBusEvent);

            using (HttpClient client = new HttpClient())
            {
                EventTriggeredDto eventTriggered = new EventTriggeredDto()
                {
                    Event = eventBusEvent.Body,
                    Payload = eventBusEvent.Body,
                    TriggerDate = DateTime.Now
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(eventTriggered), Encoding.UTF8, "application/json");

                var stringTask = await client.PostAsync("http://localhost:20005/api/v1/webhooks", stringContent).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
