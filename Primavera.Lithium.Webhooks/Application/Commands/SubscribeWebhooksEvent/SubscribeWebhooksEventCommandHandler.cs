using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Primavera.Hydrogen;
using Primavera.Hydrogen.Storage.Tables;
using Primavera.Lithium.Webhooks.Abstractions;

namespace Primavera.Lithium.Webhooks.Application
{
    public class SubscribeWebhooksEventCommandHandler : IRequestHandler<SubscribeWebhooksEventCommand, BaseResponse>
    {
        #region Private Fields

        private ITableReference table;

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        private ILogger<SubscribeWebhooksEventCommandHandler> Logger { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeWebhooksEventCommandHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">asd</param>
        /// <param name="logger">asds</param>
        public SubscribeWebhooksEventCommandHandler(IServiceProvider serviceProvider, ILogger<SubscribeWebhooksEventCommandHandler> logger)
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
        public async Task<BaseResponse> Handle(SubscribeWebhooksEventCommand request, CancellationToken cancellationToken)
        {
            SmartGuard.NotNull(() => request, request);

            SubscribeWebhooksEventDto webhooksSubscriptionDto = request.WebhooksSubscriptionDto;

            foreach (var eventSubscription in webhooksSubscriptionDto.Events)
            {
                WebhooksSubscription webhooksSubscription = new WebhooksSubscription(webhooksSubscriptionDto.Product, eventSubscription, webhooksSubscriptionDto.Subscription)
                {
                    Event = eventSubscription,
                    Product = webhooksSubscriptionDto.Product,
                    Subscription = webhooksSubscriptionDto.Subscription,
                    NotificationEndpoint = webhooksSubscriptionDto.NotificationEndpoint,
                    Properties = JsonConvert.SerializeObject(webhooksSubscriptionDto.Properties),
                };

                await this.table.InsertEntityAsync<WebhooksSubscription>(webhooksSubscription).ConfigureAwait(false);
            }

            return new BaseResponse { IsSuccess = true, Message = "WebhooksEvent(s) subscribed with success!" };
        }

        #region Private Methods

        /// <summary>
        /// Interface of the Lock Service.
        /// </summary>
        private async Task<ITableReference> GetTableAsync()
        {
            ITableStorageService service = this.ServiceProvider.GetRequiredService<ITableStorageService>();
            ITableReference table = service.GetTable("WebhooksSubscription");

            return table;
        }

        #endregion
    }
}
