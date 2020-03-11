# <a name="root"></a>PRIMAVERA Lithium Faturacao Service (FATUR) Client Library

Serviço de faturação da Primavera.

## Service Client

| Class | Description |
| - | - |
| [`FaturacaoClient`](#FaturacaoClient) | The entry point of the Faturacao Service client library. |

## Operations

| Class | Name | Description |
| - | - | - |
| [`Billing`](#BillingOperations) | Billing | Billing Controller. |
| [`Subscription`](#SubscriptionOperations) | Subscription | Webhooks Subscripiton controller. |
| [`Webhooks`](#WebhooksOperations) | Webhooks | Webhooks controller. |

## Models

| Class | Name | Description |
| - | - | - |
| [`Invoice`](#Invoice) | Invoice | The Model for the Invoice. |
| [`WebhooksEvent`](#WebhooksEvent) | Webhooks Event | Model for the Webhooks Events. |
| [`WebhooksSubscription`](#WebhooksSubscription) | Webhooks Subscription | Model for the webhooks subscriptions. |

## Enumerations

The client library has no enumerations.

## Reference

### Service Client Classes

#### <a name="FaturacaoClient"></a>`FaturacaoClient`

- Namespace: `Primavera.Lithium.Faturacao`
- Inheritance: `FaturacaoClientBase` (`ServiceClient<FaturacaoClientBase>`)

##### Constructors

###### FaturacaoClient(Uri, ServiceClientCredentials)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `credentials` | `ServiceClientCredentials` | The credentials that should be used to access the service. |

###### FaturacaoClient(Uri, ServiceClientCredentials, HttpMessageHandler)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `credentials` | `ServiceClientCredentials` | The credentials that should be used to access the service. |
| `handler` | `HttpMessageHandler` | The root message handler. |

###### FaturacaoClient(Uri, ServiceClientCredentials, HttpMessageHandler, bool)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `credentials` | `ServiceClientCredentials` | The credentials that should be used to access the service. |
| `handler` | `HttpMessageHandler` | The root message handler. |
| `disposeHandler` | `bool` | True if the inner handler should be disposed of, false if the inner handler is intended to be reused. |

###### FaturacaoClient(Uri, AuthenticationCallback)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `callback` | `AuthenticationCallback` | The callback that will be invoked during authentication to get the access token. |

###### FaturacaoClient(Uri, AuthenticationCallback, HttpMessageHandler)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `callback` | `AuthenticationCallback` | The callback that will be invoked during authentication to get the access token. |
| `handler` | `HttpMessageHandler` | The root message handler. |

###### FaturacaoClient(Uri, AuthenticationCallback, HttpMessageHandler, bool)

| Parameter | Type | Description |
| - | - | - |
| `baseUri` | `Uri` | The base URI of the service. |
| `callback` | `AuthenticationCallback` | The callback that will be invoked during authentication to get the access token. |
| `handler` | `HttpMessageHandler` | The root message handler. |
| `disposeHandler` | `bool` | True if the inner handler should be disposed of, false if the inner handler is intended to be reused. |

##### Properties

| Property | Type | Description |
| - | - | - |
| `AcceptLanguage` | `string` | Gets or sets the preferred language for the response. |

##### Methods

###### SetRetryPolicy(RetryPolicy

Sets the retry policy.

| Parameter | Type | Description |
| - | - | - |
| `retryPolicy` | `RetryPolicy` | The new retry policy. |

###### SetUserAgent(string, string, string

Sets the user agent header when making requests to the service.

| Parameter | Type | Description |
| - | - | - |
| `productName` | `string` | The product name. |
| `version` | `string` | The version. |
| `info` | `string` | Additional information. |

##### Example

```csharp
Uri address = new Uri("[service-address]");

using FaturacaoClient client = new FaturacaoClient(
    new Uri(address),
    ClientCredentials.NoCredentials);
```

[^ Back to top](#root)

### Operations Classes

#### <a name="BillingOperations"></a>`BillingOperations`

Billing Controller.

- Namespace: `Primavera.Lithium.Faturacao`
- Inheritance: `BillingOperationsBase` (`IBillingOperations`)

##### Methods

##### `CreateInvoiceAsync()`

Creates a new invoice.

```csharp
public async Task<ServiceOperationResult> CreateInvoiceAsync(IEnumerable<Invoice> invoice, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `invoice` | `IEnumerable<Invoice>` | Fatura a criar. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| None | The operation has no return value. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.NoContent` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `GetInvoicesAsync()`

Get all the invoices.

```csharp
public async Task<ServiceOperationResult<IEnumerable<Invoice>>> GetInvoicesAsync(CancellationToken cancellationToken = default);
```

###### Parameters

The operation has no parameters.


###### Returns

| Return Type | Description |
| - | - | - | - |
| `IEnumerable<Primavera.Lithium.Faturacao.Models.Invoice>` | Returns a list of invoices. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.Ok` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

#### <a name="SubscriptionOperations"></a>`SubscriptionOperations`

Webhooks Subscripiton controller.

- Namespace: `Primavera.Lithium.Faturacao`
- Inheritance: `SubscriptionOperationsBase` (`ISubscriptionOperations`)

##### Methods

##### `GetWebooksEventsForSubscriptionAsync()`

Gets WebooksEvents For Subscription.

```csharp
public async Task<ServiceOperationResult<IEnumerable<WebhooksSubscription>>> GetWebooksEventsForSubscriptionAsync(string subscription, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `subscription` | `string` | Name of the Subscription. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| `IEnumerable<Primavera.Lithium.Faturacao.Models.WebhooksSubscription>` | A list of WebhooksSubscriptions. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.Ok` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `GetWebooksSubscriptionsAsync()`

Gets all the webhooks subscriptions.

```csharp
public async Task<ServiceOperationResult<IEnumerable<WebhooksSubscription>>> GetWebooksSubscriptionsAsync(CancellationToken cancellationToken = default);
```

###### Parameters

The operation has no parameters.


###### Returns

| Return Type | Description |
| - | - | - | - |
| `IEnumerable<Primavera.Lithium.Faturacao.Models.WebhooksSubscription>` | Returns a list with all the webhooks subscriptions. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.Ok` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `SubscribeWebhooksEventAsync()`

Subscribe WebhooksEvent.

```csharp
public async Task<ServiceOperationResult> SubscribeWebhooksEventAsync(WebhooksSubscription webhooksSubscription, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `webhooksSubscription` | `WebhooksSubscription` | Webhooks Subscription. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| None | The operation has no return value. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.NoContent` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `UnsubscribeWebhooksEventAsync()`

Unsubscribe WebhooksEvent.

```csharp
public async Task<ServiceOperationResult> UnsubscribeWebhooksEventAsync(string webhooksEvent, string subscriber, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `webhooksEvent` | `string` | Name of the WebhooksEvent. | Required.  |
| `subscriber` | `string` | The event subscriber. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| None | The operation has no return value. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.NoContent` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

#### <a name="WebhooksOperations"></a>`WebhooksOperations`

Webhooks controller.

- Namespace: `Primavera.Lithium.Faturacao`
- Inheritance: `WebhooksOperationsBase` (`IWebhooksOperations`)

##### Methods

##### `CreateWebhooksEventAsync()`

Create Webhooks Event.

```csharp
public async Task<ServiceOperationResult> CreateWebhooksEventAsync(WebhooksEvent webhooksEvent, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `webhooksEvent` | `WebhooksEvent` | Webhook event being created. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| None | The operation has no return value. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.NoContent` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `DeleteWebooksEventAsync()`

Deletes WebooksEvent.

```csharp
public async Task<ServiceOperationResult> DeleteWebooksEventAsync(string publisher, string webhooksEvent, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `publisher` | `string` | The name of the publisher. | Required.  |
| `webhooksEvent` | `string` | Name of the WebhooksEvent. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| None | The operation has no return value. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.NoContent` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `GetWebhooksEventsAsync()`

Get Webhooks Events.

```csharp
public async Task<ServiceOperationResult<IEnumerable<WebhooksEvent>>> GetWebhooksEventsAsync(CancellationToken cancellationToken = default);
```

###### Parameters

The operation has no parameters.


###### Returns

| Return Type | Description |
| - | - | - | - |
| `IEnumerable<Primavera.Lithium.Faturacao.Models.WebhooksEvent>` | Returns a list of webhooks events. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.Ok` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

##### `GetWebooksEventsByPublisherAsync()`

Get WebooksEvents By Publisher.

```csharp
public async Task<ServiceOperationResult<IEnumerable<string>>> GetWebooksEventsByPublisherAsync(string publisher, CancellationToken cancellationToken = default);
```

###### Parameters

| Parameter | Type | Description | Rules |
| - | - | - | - |
| `publisher` | `string` | The name of the publisher. | Required.  |


###### Returns

| Return Type | Description |
| - | - | - | - |
| `IEnumerable<string>` | A list of WebhooksEvents produced by publisher. |


###### Status Codes

| Status Code | Description |
| - | - | - | - |
| `HttpStatusCode.Ok` | Success. |
| `HttpStatusCode.BadRequest` | Failure: the request is invalid.|

> The operation will raise `ServiceException` for any failure status code. The exception may include a body with a `ServiceError` depending on the status code.


[^ Back to top](#root)

### Models Classes

#### <a name="Invoice"></a>`Invoice`

The Model for the Invoice.

- Namespace: `Primavera.Lithium.Faturacao.Models`
- Inheritance: `InvoiceBase`

##### Properties

| Property | Type | Description | Rules |
| - | - | - | - |
| `Id` | `Guid` | The ID of the invoice. |  |
| `Amount` | `string` | The amount to pay. | Required.  |

[^ Back to top](#root)

#### <a name="WebhooksEvent"></a>`WebhooksEvent`

Model for the Webhooks Events.

- Namespace: `Primavera.Lithium.Faturacao.Models`
- Inheritance: `WebhooksEventBase`

##### Properties

| Property | Type | Description | Rules |
| - | - | - | - |
| `Event` | `string` | Name of the webhook event. | Required.  |
| `Publisher` | `string` | Publisher of an event. | Required.  |
| `RequiredScope` | `string` | Required scope to subscribe the event. | Required.  |

[^ Back to top](#root)

#### <a name="WebhooksSubscription"></a>`WebhooksSubscription`

Model for the webhooks subscriptions.

- Namespace: `Primavera.Lithium.Faturacao.Models`
- Inheritance: `WebhooksSubscriptionBase`

##### Properties

| Property | Type | Description | Rules |
| - | - | - | - |
| `Event` | `string` | ID of the event being subscribed. | Required.  |
| `Subscriber` | `string` | ID of the Subscriber. | Required.  |
| `NotificationEndpoint` | `string` | Endpoint to where the notification is sent. | Required.  |

[^ Back to top](#root)

