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
using Primavera.Lithium.Faturacao.WebApi.Abstractions;

namespace Primavera.Lithium.Faturacao.WebApi.Application
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public class GetWebhooksEventsByProductQueryHandler : IRequestHandler<GetWebhooksEventsByProductQuery, IEnumerable<WebhooksEvent>>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<GetWebhooksEventsByProductQueryHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWebhooksEventsByProductQueryHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public GetWebhooksEventsByProductQueryHandler(IServiceProvider serviceProvider, ILogger<GetWebhooksEventsByProductQueryHandler> logger)
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
        public async Task<IEnumerable<WebhooksEvent>> Handle(GetWebhooksEventsByProductQuery request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            return await this.table.RetrieveEntitiesAsync<WebhooksEvent>(request.Product).ConfigureAwait(false);
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
