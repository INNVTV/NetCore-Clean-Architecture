# NetCore-CQRS
Example set of projects showcasing the use of CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with NSwag and CosmosDB for data with .Net Core

## CQRS

Command Query Responsibility Segregation is handled by...

## Event Sourcing

NetCore-CleanArchitecture-Framework

## MediatR

## Cross Cutting Concerns

Cross cutting concerns such as logging, authorization, etc... are handled by:

## Service COmmunication
gRPC/API/nSwag

## CosmosDB Document Partitioning Strategy
Our strategy is to use **'_docType'** as our partition.
 * Account documents are lableled **"Account"**
 * Platform documents **"Platform"**
 * Documents belonging to accounts are labeled **"Account-<AccountID>"**
 * _docType for a specific Entity type are labeled **"<EntityName>"**
 * Documents belonging to an entity are named **"EntityName-<EntityId>"**
