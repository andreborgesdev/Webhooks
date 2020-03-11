using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Primavera.Hydrogen.AspNetCore.Hosting;

namespace Primavera.Lithium.Webhooks.BackgroundServices
{
    public class WebhooksQueuedBackgroundService : QueuedBackgroundServiceWithWorker<SendWebhooksToSubscriptionsWorker>
    {
        #region Public Constructor

        public WebhooksQueuedBackgroundService(IServiceProvider serviceProvider, ILogger<WebhooksQueuedBackgroundService> logger)
            : base(serviceProvider, logger)
        {
        }

        #endregion
        #region Public Properties

        /// <inheritdoc />
        public override IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker> Queue => this.ServiceProvider.GetRequiredService<IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>>();

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        ////protected async override Task ExecuteWorkAsync(SendWebhooksToSubscriptionsWorker workItem, CancellationToken cancellationToken)
        ////{
        ////    Console.WriteLine("Testeee");

        ////    await workItem.ExecuteAsync(cancellationToken);

        ////    return;
        ////}

        #endregion
    }
}
