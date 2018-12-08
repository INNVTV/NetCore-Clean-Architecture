# NetCore-CQRS
Example set of projects showcasing the use of CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with NSwag and CosmosDB for data with .Net Core

Based on Jason Taylors talk on "Clean Architecture": https://www.youtube.com/watch?v=_lwCVE_XgqI

![Architecture](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/clean-architecture.png)

# Domain layer

## CQRS

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
 
 # Application Layer
 
 # Services Layer
 Core.Services Project
 
 ## REST APIs
 Located within "Controllers" folder
 
 ## RPC
gRPC implementation. Used by background workers and custodians. Custodial and Platform calls are initited through here.

## WebHooks
APIs for integration with 3rd party systems such as Stripe or Event Grid.
 
