////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading;
////using System.Threading.Tasks;
////using Microsoft.Extensions.DependencyInjection;
////using Microsoft.Extensions.Logging;
////using Primavera.Hydrogen.Storage.Tables;

////namespace Primavera.Lithium.Faturacao.WebApi.BackgroundServices
////{
////    /// <summary>
////    /// EntityJsonPropertyConverter attribute.
////    /// </summary>
////    internal partial class SendEventToSubscriptionsService
////    {
////        #region Private Fields

////        ////private ITableReference table;

////        #endregion

////        #region Private Properties

////        private IServiceProvider ServiceProvider { get; }

////        private ILogger<SendEventToSubscriptionsService> Logger { get; set; }

////        #endregion

////        #region Protected Methods

////        /// <inheritdoc/>
////        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
////        {
////            ////this.table = await this.GetTableAsync().ConfigureAwait(false);
////            await Task.Delay(100);
////        }

////        #endregion

////        #region Private Methods

////        /// <summary>
////        /// Interface of the Lock Service.
////        /// </summary>
////        ////private async Task<ITableReference> GetTableAsync()
////        ////{
////        ////    ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
////        ////    ITableReference table = service.GetTable("WebhooksSubscription");

////        ////    await table.CreateIfNotExistsAsync().ConfigureAwait(false);

////        ////    return table;
////        ////}

////        #endregion
////    }
////}
