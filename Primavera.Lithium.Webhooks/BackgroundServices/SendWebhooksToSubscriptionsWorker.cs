using Microsoft.Extensions.Logging;
using Primavera.Hydrogen.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
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

        #endregion     

        #region Public Methods

        /// <inheritdoc />
        public async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("EXECUTIIIIIIIIIIIING");

            //IEnumerable<SmsNotificationStateData> records = await this.SmsNotificationReceiversManager.GetQueuedAsync().ConfigureAwait(false);

            return;
        }

        #endregion
    }
}
