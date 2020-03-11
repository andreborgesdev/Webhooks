using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Primavera.Hydrogen.AspNetCore.Hosting;
using Primavera.Lithium.Webhooks.Abstractions;
using Primavera.Lithium.Webhooks.Application;

namespace Primavera.Lithium.Webhooks.BackgroundServices
{
    public class RetryToSendWebhooksToSubscriptionsService : TimedBackgroundService
    {
        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<RetryToSendWebhooksToSubscriptionsService> Logger { get; set; }

        private IMediator Mediator
        {
            get
            {
                return this.ServiceProvider.GetRequiredService<IMediator>();
            }
        }

        #endregion

        #region Public Properties

        public override TimeSpan WaitPeriod
        {
            get
            {
                return TimeSpan.FromSeconds(60);
            }
        }

        #endregion

        #region Public Constructor

        public RetryToSendWebhooksToSubscriptionsService(IServiceProvider serviceProvider, ILogger<RetryToSendWebhooksToSubscriptionsService> logger)
            : base(serviceProvider, logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
        }

        #endregion

        #region Protected Methods

        protected async override Task ExecuteWorkAsync(CancellationToken cancellationToken)
        {
            IEnumerable<WebhooksEventLog> webhooksEventsLog = await Mediator.Send(new GetWebhooksEventLogQuery());
            webhooksEventsLog.toList();
        }

        #endregion

    }
}
