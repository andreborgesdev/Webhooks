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

            //if (cancellationToken.IsCancellationRequested)
            //{
            //    return;
            //}

            //await SendEventToSubscribers();
        }

        public async Task Executar(WebhooksEventLog webhooksEventLog)
        {
            await this.SendEventToSubscribers(webhooksEventLog);
        }

        public async Task SendEventToSubscribers(WebhooksEventLog webhooksEventLog)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ++webhooksEventLog.Retries;

                    //var stringContent = new StringContent(JsonConvert.SerializeObject(webhooksEventLog.EventPayload), Encoding.UTF8, "application/json");
                    var stringContent = new StringContent(webhooksEventLog.EventPayload, Encoding.UTF8, "application/json");

                    var request = await client.PostAsync(webhooksEventLog.NotificationEndpoint, stringContent).ConfigureAwait(false);

                    if (request.IsSuccessStatusCode)
                    {
                        webhooksEventLog.DeliveredOn = DateTime.Now;
                        webhooksEventLog.Success = true;
                        await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = webhooksEventLog });
                    }
                    else
                    {
                        webhooksEventLog.NextRetry = DateTime.Now.AddSeconds(5);
                        await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = webhooksEventLog });
                    }
                }
            }
            catch (Exception e)
            {
                webhooksEventLog.NextRetry = DateTime.Now.AddSeconds(5);
                await Mediator.Send(new UpdateWebhooksEventLogCommand() { WebhooksEventLog = webhooksEventLog });
            }
        }

        #endregion
    }
}
