using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class SaveWebhooksEventLogCommandHandler : IRequestHandler<SaveWebhooksEventLogCommand, BaseResponse>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<SaveWebhooksEventLogCommandHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveWebhooksEventLogCommandHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public SaveWebhooksEventLogCommandHandler(IServiceProvider serviceProvider, ILogger<SaveWebhooksEventLogCommandHandler> logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
            this.table = this.GetTableAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Defines base class for the controller that provides monitoring routes.
        /// </summary>
        /// <param name="request">qewe</param>
        /// <param name="cancellationToken">ad</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<BaseResponse> Handle(SaveWebhooksEventLogCommand request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            WebhooksEventLog webhooksEventLog = new WebhooksEventLog(request.Product, request.Event)
            {
                Event = request.Event,
                Product = request.Product,
                Subscription = request.Subscription,
                NotificationEndpoint = request.NotificationEndpoint,
                Success = request.Success,
                Retries = 0,
                NextRetry = request.Success ? DateTime.Now : DateTime.Now.AddSeconds(5),
                DeliveredOn = request.Success ? DateTime.Now : DateTime.Now.AddSeconds(5),
                EventPayload = JsonConvert.SerializeObject(request.EventPayload)
            };

            await this.table.InsertEntityAsync<WebhooksEventLog>(webhooksEventLog).ConfigureAwait(false);

            return new BaseResponse { IsSuccess = true, Message = "WebhooksEventLog created with success!" };
        }

        #region Private Methods

        /// <summary>
        /// Interface of the Lock Service.
        /// </summary>
        private async Task<ITableReference> GetTableAsync()
        {
            ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
            ITableReference table = service.GetTable("WebhooksEventLog");

            return table;
        }

        #endregion
    }
}
