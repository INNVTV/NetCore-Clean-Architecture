# NetCore-CQRS

## CQRS

Command Query Responsibility Segregation is handled by...

## Event Sourcing

NetCore-CleanArchitecture-Framework

## MediatR

## Cross Cutting Concerns

Cross cutting concerns such as logging, authorization, etc... are handled by:

## Service COmmunication
gRPC/API/nSwag

## CosmosDb/DocumentDB Partitioning
Our strategy id oto use _docType. Account documents are lableled "Account" Platform documents "Platform", documents belonging to accounts are labeled "Account-<AccountID>" _docType for a specific Entity type are labeled "<EntityName>" and documents belonging to that entity are named "EntityName-<EntityId>"
