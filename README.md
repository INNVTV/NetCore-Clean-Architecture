# NetCore-CQRS
Example set of projects showcasing the use of CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with NSwag and CosmosDB for data with .Net Core

## CQRS

Command Query Responsibility Segregation is handled by...

## Event Sourcing
xxxx

## MediatR
xxxx

## Cross Cutting Concerns
Cross cutting concerns such as logging, authorization, etc... are handled by:

## ViewModels
View models that are returned from Query methods will include UI related values such as "canDelete" and "canEdit"

## Service-to-service Communication
gRPC/API/nSwag

## Containerization
Docker and Docker Compose is used to help manage local builds and multi-enviornment configuration.

## Configuratin
We use .Net Cores built in with Docker and Docker compose helping to manage builds for specific enviornments

## Logging
Logging is handled by Table Storage. We log by Time, Activity, Account and this can be used as an example for adding additional dimensions.

Logging is built into all Command related methods via MediatR

## Authorization
.Net Core Identity is used. Users are assigned to a Account object.

Authorization is built into all Command related methods via MediatR

## CosmosDB Document Partitioning Strategy
Our strategy is to use **'_docType'** as our partition.
 * Account documents are lableled **"Account"**
 * Platform documents **"Platform"**
 * Documents belonging to accounts are labeled **"Account-\<AccountID\>"**
 * _docType for a specific Entity type are labeled **"\<EntityName\>"**
 * Documents belonging to an entity are named **"EntityName-\<EntityId\>"**
