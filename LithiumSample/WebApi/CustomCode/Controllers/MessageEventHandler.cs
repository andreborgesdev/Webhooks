using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.EventBus.Contracts;
using Primavera.Hydrogen.EventBus.Exceptions;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Faturacao.WebApi.Abstractions;
using Primavera.Lithium.Faturacao.WebApi.Managers;

namespace Primavera.Lithium.Faturacao.WebApi.Controllers
{
    /// <summary>
    /// The message event handler.
    /// </summary>
    /// <seealso cref="Primavera.Hydrogen.EventBus.IEventBusEventHandler{T}"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>")]
    public class MessageEventHandler : IEventBusEventHandler<WebhooksSubscription>
    {
        #region Private Fields

        private ITableReference table;
        private IHttpContextAccessor httpContextAccessor;

        #endregion

        #region Private Properties

        private HttpContext HttpContext
        {
            get
            {
                if (this.httpContextAccessor == null)
                {
                    this.httpContextAccessor = (IHttpContextAccessor)this.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));
                }

                return this.httpContextAccessor.HttpContext;
            }
        }

        private IServiceProvider ServiceProvider { get; }

        private ILogger<WebhooksSubscriptionManager> Logger { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventHandler"/> class.
        /// </summary>
        public MessageEventHandler()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the specified message event.
        /// </summary>
        /// <param name="eventBusEvent">The message event.</param>
        /// <returns>Boolean according execution success.</returns>
        public async Task<bool> Handle(IEventBusEvent<WebhooksSubscription> eventBusEvent)
        {
            SmartGuard.NotNull(() => eventBusEvent, eventBusEvent);

            bool success = false;   
            var retryStrategy = new ExponentialBackoffRetryStrategy();

            try
            {
                IEnumerable<WebhooksSubscription> eventSubcriptions = await this.GetAllSubscribersForAnEvent(eventBusEvent.Body.Product, eventBusEvent.Body.Event).ConfigureAwait(false);
                
                do
                {
                    var result = await this.SendEventToSubscribersTeste(eventSubcriptions.ToList()).ConfigureAwait(false);
                    success = result;
                }
                while (!success);

                return true;
            }
            catch (EventBusServiceException e)
            {
                Console.WriteLine($"Message handler has encountered an exception: '{e.Message}'");

                return false;
            }
        }

        /// <summary>
        /// Handles the specified message event.
        /// </summary>
        /// <param name="webhooksSubscriptions">The message event.</param>
        /// <returns>Boolean according execution success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<bool> SendEventToSubscribersTeste(List<WebhooksSubscription> webhooksSubscriptions)
        {
            SmartGuard.NotNull(() => webhooksSubscriptions, webhooksSubscriptions);

            foreach (var subscription in webhooksSubscriptions)
            {
                using (HttpClient client = new HttpClient())
                {
                    EventTriggeredDto eventTriggered = new EventTriggeredDto()
                    {
                        Event = subscription.Event,
                        Payload = subscription.Event,
                        TriggerDate = DateTime.Now
                    };

                    var stringContent = new StringContent(JsonConvert.SerializeObject(eventTriggered), Encoding.UTF8, "application/json");

                    var stringTask = await client.PostAsync("http://localhost:20005/api/v1/webhooks", stringContent).ConfigureAwait(false);
                }
            }

            return true;
        }

        /// <summary>
        /// Handles the specified message event.
        /// </summary>
        /// <param name="product">The product event.</param>
        /// <param name="eventTriggered">The message event.</param>
        /// <returns>Boolean according execution success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<IEnumerable<WebhooksSubscription>> GetAllSubscribersForAnEvent(string product, string eventTriggered)
        {
            var p = product;
            var e = eventTriggered;
            this.table = await this.GetTableAsync().ConfigureAwait(false);

            List<WebhooksSubscription> lista = new List<WebhooksSubscription>();

            return lista;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Interface of the Lock Service.
        /// </summary>
        private async Task<ITableReference> GetTableAsync()
        {
            ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
            ITableReference table = service.GetTable("WebhooksSubscription");

            await table.CreateIfNotExistsAsync().ConfigureAwait(false);

            return table;
        }

        #endregion
    }
}
