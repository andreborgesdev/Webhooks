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
    public class GetWebhooksEventLogQueryHandler : IRequestHandler<GetWebhooksEventLogQuery, IEnumerable<WebhooksEventLog>>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<GetWebhooksEventLogQueryHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWebhooksEventLogQueryHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public GetWebhooksEventLogQueryHandler(IServiceProvider serviceProvider, ILogger<GetWebhooksEventLogQueryHandler> logger)
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
        public async Task<IEnumerable<WebhooksEventLog>> Handle(GetWebhooksEventLogQuery request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            return await this.table.RetrieveEntitiesAsync<WebhooksEventLog>().ConfigureAwait(false);
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
