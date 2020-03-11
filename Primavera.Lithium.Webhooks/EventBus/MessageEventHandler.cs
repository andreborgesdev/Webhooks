using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Hydrogen.EventBus;
using Primavera.Hydrogen.Policies.Retry.Strategies;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;
using Primavera.Lithium.Webhooks.BackgroundServices;

namespace Primavera.Lithium.Webhooks.EventBus
{
    public class MessageEventHandler : IEventBusEventHandler<EventTriggeredDto>
    {
        private IServiceProvider ServiceProvider { get; }

        private IMediator Mediator 
        { 
            get 
            {
                return this.ServiceProvider.GetRequiredService<IMediator>();
            } 
        }


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventHandler"/> class.
        /// </summary>
        public MessageEventHandler(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        #endregion

        private IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker> WorkerQueue
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>>();
            }
        }

        #region Public Methods

        /// <summary>
        /// Handles the specified message event.
        /// </summary>
        /// <param name="eventBusEvent">The message event.</param>
        /// <returns>Boolean according execution success.</returns>
        public async Task<bool> Handle(IEventBusEvent<EventTriggeredDto> eventBusEvent)
        {
            SmartGuard.NotNull(() => eventBusEvent, eventBusEvent);

            bool success = false;
            var retryStrategy = new ExponentialBackoffRetryStrategy();

            try
            {
                ////var worker = this.ServiceProvider.GetService<SendWebhooksToSubscriptionsWorker>();

                ////this.WorkerQueue.Enqueue(
                ////    new SendWebhooksToSubscriptionsWorker(
                ////        this.ServiceProvider,
                ////        this.ServiceProvider.GetRequiredService<ILogger<SendWebhooksToSubscriptionsWorker>>()));
                ///

                IEnumerable<WebhooksSubscription> eventSubcriptions = await this.GetWebhooksSubscriptionsByEvent(eventBusEvent.Body.Product, eventBusEvent.Body.Event).ConfigureAwait(false);
                
                await SendEventToSubscribers(eventSubcriptions, eventBusEvent.Body.Product, eventBusEvent.Body.Event, eventBusEvent.Body);

                Console.WriteLine($"Incoming message: '{eventBusEvent.Body.Payload}'");
                
                return true;
            }
            catch (EventBusException e)
            {
                Console.WriteLine($"Message handler has encountered an exception: '{e.Message}'");

                return false;
            }
        }

        public async Task<IEnumerable<WebhooksSubscription>> GetWebhooksSubscriptionsByEvent(string product, string eventTriggered)
        {
            return await this.Mediator.Send(new GetWebhooksSubscriptionsByEventQuery() { Product = product, Event = eventTriggered });
        }

        public async Task SendEventToSubscribers(IEnumerable<WebhooksSubscription> webhooksSubscriptions, string product, string eventTriggered, EventTriggeredDto payload)
        {
            SmartGuard.NotNull(() => webhooksSubscriptions, webhooksSubscriptions);

            foreach (var subscription in webhooksSubscriptions)
            {
                await SaveWebhooksEventLog(eventTriggered, product, subscription.Subscription, payload);

                using (HttpClient client = new HttpClient())
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var stringTask = await client.PostAsync(subscription.NotificationEndpoint, stringContent).ConfigureAwait(false);
                }
            }
        }

        public async Task SaveWebhooksEventLog(string webhooksEvent, string product, string subscription, EventTriggeredDto payload)
        {
            SmartGuard.NotNull(() => payload, payload);

            await this.Mediator.Send(new SaveWebhooksEventLogCommand() { Event = webhooksEvent, Product = product, Subscription = subscription, EventPayload = payload });
        }

        #endregion
    }
}
