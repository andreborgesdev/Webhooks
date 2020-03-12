using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Hydrogen.EventBus;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Primavera.Lithium.Webhooks.BackgroundServices
{
    public class SendWebhooksToSubscriptionsWorker : BackgroundWorker
    {
        #region Public Constructor

        public SendWebhooksToSubscriptionsWorker(IServiceProvider serviceProvider, ILogger<SendWebhooksToSubscriptionsWorker> logger)
            : base(serviceProvider, logger)
        {
        }

        private IMediator Mediator
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IMediator>();
            }
        }

        #endregion     

        #region Public Methods

        /// <inheritdoc />
        public async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("EXECUTIIIIIIIIIIIING");

            //IEnumerable<SmsNotificationStateData> records = await this.SmsNotificationReceiversManager.GetQueuedAsync().ConfigureAwait(false);

            //try
            //{
            //    IEnumerable<WebhooksSubscription> eventSubcriptions = await this.GetWebhooksSubscriptionsByEvent(eventBusEvent.Body.Product, eventBusEvent.Body.Event).ConfigureAwait(false);

            //    await SendEventToSubscribers(eventSubcriptions, eventBusEvent.Body.Product, eventBusEvent.Body.Event, eventBusEvent.Body);

            //    Console.WriteLine($"Incoming message: '{eventBusEvent.Body.Payload}'");

            //    return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Message handler has encountered an exception: '{e.Message}'");

            //    return false;
            //}
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
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                        var request = await client.PostAsync(subscription.NotificationEndpoint, stringContent).ConfigureAwait(false);

                        if (request.IsSuccessStatusCode)
                        {
                            await SaveWebhooksEventLog(eventTriggered, product, subscription.Subscription, subscription.NotificationEndpoint, payload, true);
                        }
                        else
                        {
                            await SaveWebhooksEventLog(eventTriggered, product, subscription.Subscription, subscription.NotificationEndpoint, payload, false);
                        }
                    }
                }
                catch (Exception e)
                {
                    await SaveWebhooksEventLog(eventTriggered, product, subscription.Subscription, subscription.NotificationEndpoint, payload, false);
                }
            }
        }

        public async Task SaveWebhooksEventLog(string webhooksEvent, string product, string subscription, string notificationEndpoint, EventTriggeredDto payload, bool success)
        {
            SmartGuard.NotNull(() => payload, payload);

            await this.Mediator.Send(new SaveWebhooksEventLogCommand()
            {
                Event = webhooksEvent,
                Product = product,
                Subscription = subscription,
                NotificationEndpoint = notificationEndpoint,
                EventPayload = payload,
                Success = success
            });
        }

        #endregion
    }
}
