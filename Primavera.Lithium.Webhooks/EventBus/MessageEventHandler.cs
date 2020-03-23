using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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

        public SendWebhooksToSubscriptionsWorker Worker
        {
            get
            {
                return new SendWebhooksToSubscriptionsWorker(
                    this.ServiceProvider,
                    this.ServiceProvider.GetRequiredService<ILogger<SendWebhooksToSubscriptionsWorker>>());
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
                //this.WorkerQueue.Enqueue(
                //    new SendWebhooksToSubscriptionsWorker(
                //        this.ServiceProvider,
                //        this.ServiceProvider.GetRequiredService<ILogger<SendWebhooksToSubscriptionsWorker>>()));

                await Worker.Executar(eventBusEvent.Body);

                Console.WriteLine($"Incoming message: '{eventBusEvent.Body.Payload}'");
                
                return true;
            }
            catch (EventBusException e)
            {
                Console.WriteLine($"Message handler has encountered an exception: '{e.Message}'");

                return false;
            }
        }

        #endregion
    }
}
