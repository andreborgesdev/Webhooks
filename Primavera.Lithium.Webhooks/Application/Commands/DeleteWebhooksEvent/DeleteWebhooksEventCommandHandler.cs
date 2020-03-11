using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Primavera.Hydrogen;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public class DeleteWebhooksEventCommandHandler : IRequestHandler<DeleteWebhooksEventCommand, BaseResponse>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<DeleteWebhooksEventCommandHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWebhooksEventCommandHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public DeleteWebhooksEventCommandHandler(IServiceProvider serviceProvider, ILogger<DeleteWebhooksEventCommandHandler> logger)
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
        public async Task<BaseResponse> Handle(DeleteWebhooksEventCommand request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            WebhooksEvent eventToDelete = await this.table.RetrieveEntityAsync<WebhooksEvent>(request.Product, request.WebhooksEvent).ConfigureAwait(false);

            await this.table.DeleteEntityAsync<WebhooksEvent>(eventToDelete).ConfigureAwait(false);

            return new BaseResponse { IsSuccess = true, Message = "WebhooksEvent deleted with success!" };
        }

        #region Private Methods

        /// <summary>
        /// Interface of the Lock Service.
        /// </summary>
        private async Task<ITableReference> GetTableAsync()
        {
            ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
            ITableReference table = service.GetTable("WebhooksEvent");

            return table;
        }

        #endregion
    }
}
