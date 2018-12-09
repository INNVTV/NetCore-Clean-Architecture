# NetCore-Clean-Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with both REST and gRPC endpoints, CosmosDB for data and Table Storage for logging.

Based on Jason Taylors talk on "Clean Architecture": https://www.youtube.com/watch?v=_lwCVE_XgqI

![Architecture](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/clean-architecture.png)

# The Domain layer
 Core.Domain Project
 
### CQRS

Command Query Responsibility Segregation is handled by...

## Event Sourcing
xxxx

## MediatR
xxxx

## Cross Cutting Concerns
Cross cutting concerns such as logging, caching, authorization, etc... are handled by:

## ViewModels
View models that are returned from Query methods will include UI related values such as "canDelete" and "canEdit"

## Service-to-service Communication
Examples of clients accessing the service layer are shown in both REST and gRPC flavors.

## Containerization
Docker and Docker Compose is used to help manage local builds and multi-enviornment configuration.

## Configuratin
We use .Net Cores built in with Docker and Docker compose helping to manage builds for specific enviornments

## Logging
Logging is handled by Table Storage. We log by Time, Activity, Account and this can be used as an example for adding additional dimensions.

Logging is built into all Command related methods via MediatR

## Authorization
.Net Core Identity is used. (...or ADB2C) Users are assigned to a Account object.

Authorization is built into all Command related methods via MediatR

## CosmosDB Document Partitioning Strategy
Our strategy is to use **'_docType'** as our partition.

 * Account documents are named **"Account"**
 * Platform documents are named **"Platform"**
 * Documents belonging to an Account are named **"Account\<AccountId\>"**
 * Documents for a specific entity type are named **"\<EntityName\>"**
 * Documents belonging to an entity are named **"EntityName-\<EntityId\>"**
 * Documents of a particular entity type belonging to a spcific account are named **"EntityName-Account-\<AccountId\>"**
 * Documents belonging to particular entity type for a specific account are named **"EntityName-\<EntityId\>-Account-\<AccountId\>"**
 
 # The Application Layer
 Core.Application Project
 
 # The Services Layer
 Core.Services Project
 
 ## REST APIs
 Located within "Controllers" folder
 
     REST Endpoints: /api
 
 ## RPC
gRPC implementation. Used by background workers and custodians. Custodial and Platform calls are initited through here.

    gRPC Endpoints: /rpc

## WebHooks
APIs for integration with 3rd party systems such as Stripe or Event Grid.

    Webhoo Endpoints: /webhooks
 

![Deploy](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/typical-setup.png)