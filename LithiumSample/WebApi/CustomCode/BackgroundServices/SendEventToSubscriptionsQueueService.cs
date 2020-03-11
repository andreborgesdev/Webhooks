////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading;
////using System.Threading.Tasks;
////using Microsoft.Extensions.DependencyInjection;
////using Primavera.Hydrogen.AspNetCore.Hosting;

////namespace Primavera.Lithium.Faturacao.WebApi.BackgroundServices
////{
////    /// <inheritdoc />
////    internal partial class SendEventToSubscriptionsQueueService
////    {
////        #region Public Properties

////        /// <inheritdoc />
////        public override IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker> Queue => this.ServiceProvider.GetRequiredService<IBackgroundWorkQueue<SendWebhooksToSubscriptionsWorker>>();

////        #endregion

////        #region Protected Methods

////        /// <inheritdoc />
////        protected override Task ExecuteWorkAsync(SendWebhooksToSubscriptionsWorker workItem, CancellationToken cancellationToken)
////        {
////            // (...)

////            return Task.CompletedTask;
////        }

////        #endregion
////    }
////}
