using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.AspNetCore.Hosting;
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
    public class RetryToSendWebhooksToSubscriptionsWorker : BackgroundWorker
    {
        public WebhooksEventLog WebhooksEventLog { get; set; }

        #region Public Constructor

        public RetryToSendWebhooksToSubscriptionsWorker(IServiceProvider serviceProvider, ILogger<RetryToSendWebhooksToSubscriptionsWorker> logger)
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

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await SendEventToSubscribers();
        }

        public async Task SendEventToSubscribers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ++WebhooksEventLog.Retries;

                    var stringContent = new StringContent(JsonConvert.SerializeObject(WebhooksEventLog.EventPayload), Encoding.UTF8, "application/json");

                    var request = await client.PostAsync(WebhooksEventLog.NotificationEndpoint, stringContent).ConfigureAwait(false);

                    if (request.IsSuccessStatusCode)
                    {
                        WebhooksEventLog.DeliveredOn = DateTime.Now;
                        WebhooksEventLog.Success = true;
                        await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = WebhooksEventLog });
                    }
                    else
                    {
                        WebhooksEventLog.NextRetry = DateTime.Now.AddSeconds(5);
                        await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = WebhooksEventLog });
                    }
                }
            }
            catch (Exception e)
            {
                WebhooksEventLog.NextRetry = DateTime.Now.AddSeconds(5);
                await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = WebhooksEventLog });
            }
        }

        #endregion
    }
}
