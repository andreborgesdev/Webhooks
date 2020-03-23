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
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;

namespace Primavera.Lithium.Webhooks.BackgroundServices
{
    public class RetryToSendWebhooksToSubscriptionsService : TimedBackgroundServiceWithWorker<RetryToSendWebhooksToSubscriptionsWorker>
    {
        #region Private Properties

        private IMediator Mediator
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IMediator>();
            }
        }

        private IBackgroundWorkQueue<RetryToSendWebhooksToSubscriptionsWorker> WorkerQueue
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IBackgroundWorkQueue<RetryToSendWebhooksToSubscriptionsWorker>>();
            }
        }

        #endregion

        #region Public Properties

        public override TimeSpan WaitPeriod
        {
            get
            {
                return TimeSpan.FromSeconds(10);
            }
        }

        public override RetryToSendWebhooksToSubscriptionsWorker Worker
        {
            get
            {
                return new RetryToSendWebhooksToSubscriptionsWorker(
                    this.ServiceProvider,
                    this.ServiceProvider.GetRequiredService<ILogger<RetryToSendWebhooksToSubscriptionsWorker>>());
            }
        }

        #endregion

        #region Public Constructor

        public RetryToSendWebhooksToSubscriptionsService(IServiceProvider serviceProvider, ILogger<RetryToSendWebhooksToSubscriptionsService> logger)
            : base(serviceProvider, logger)
        {
            //this.ServiceProvider = serviceProvider;
            //this.Logger = logger;
        }

        #endregion

        #region Protected Methods

        protected async override Task ExecuteWorkAsync(CancellationToken cancellationToken)
        {
            IEnumerable<WebhooksEventLog> webhooksEventsLog = await Mediator.Send(new GetWebhooksEventLogQuery());

            foreach (var webhooksEventLog in webhooksEventsLog)
            {
                if (!webhooksEventLog.Success && DateTime.Now >= webhooksEventLog.NextRetry)
                {
                    await this.Worker.Executar(webhooksEventLog);
                    //await this.Worker.ExecuteAsync(cancellationToken);

                    //RetryToSendWebhooksToSubscriptionsWorker worker = this.ServiceProvider.GetRequiredService<RetryToSendWebhooksToSubscriptionsWorker>();

                    //await Worker.ExecuteAsync(cancellationToken);
                    
                    //this.WorkerQueue.Enqueue(worker);
                }
            }
        }

        #endregion

    }
}
