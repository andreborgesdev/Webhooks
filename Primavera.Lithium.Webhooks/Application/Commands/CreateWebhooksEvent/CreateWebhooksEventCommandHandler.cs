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
    public class CreateWebhooksEventCommandHandler : IRequestHandler<CreateWebhooksEventCommand, BaseResponse>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<CreateWebhooksEventCommandHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateWebhooksEventCommandHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public CreateWebhooksEventCommandHandler(IServiceProvider serviceProvider, ILogger<CreateWebhooksEventCommandHandler> logger)
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
        public async Task<BaseResponse> Handle(CreateWebhooksEventCommand request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            WebhooksEvent webhooksEvent = request.WebhooksEvent;

            await this.table.InsertEntityAsync<WebhooksEvent>(webhooksEvent).ConfigureAwait(false);

            return new BaseResponse { IsSuccess = true, Message = "WebhooksEvent created with success!" };
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
