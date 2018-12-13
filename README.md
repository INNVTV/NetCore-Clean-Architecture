# .Net Core Clean Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with both REST and gRPC endpoints, FluentValidation, CosmosDB for data and Table Storage for logging.

Based on Jason Taylors talk on "Clean Architecture": https://www.youtube.com/watch?v=_lwCVE_XgqI

![Architecture](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/clean-architecture.png)

# CoreServices
Centralized business logic. Visual Studio Solution Project. Deployable to Linux or Windows as a Docker contianer to a variety of platforms including:
 * Azure, AWS, Google or any major cloud provider
 * Virtual machines or clusters
 * Azure Web Apps, Amazon EC2 or Google AppEngine
 * Azure Service Fabric
 * Azure Service Fabric Mesh
 * Kubernetes
...or any container orchastrator of your choice.

# WebClients
UI client(s) that connect to CoreServices via REST API. Visual Studio Code projects.

# TaskClients
Background tasks hosted as workers that connect to CoreServices via gRPC. Visual Studio Code projects.

## Core.Services
Main entry point. Includes Core.Common, Core.Application and Core.Domain Projects and wraps them into a gateway for access by the clients.

DependecyInjection is handled by default .Net Core ServiceProvider. Console and Test entry points are provided in the Utilities folder.

More details can be found in the [ReadMe](CoreServices/README.md) doc for the CoreServices solution.

 
### CQRS
The Command Query Responsibility Segregation pattern is used for all access to Core.Domain through the Core.Application project as a gateway.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html

![CQRS](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs.png)

### ViewModels
View models that are returned from Query methods will include UI related values such as "canDelete" and "canEdit"

### Service-to-service Communication
Examples of clients accessing the service layer are shown in both REST and gRPC flavors.

### Containerization
Docker is used on all projects/solutions to manage local builds and deploy to multi-enviornment configurations.

## Configuration
We use .Net Cores built in with Docker and Docker compose helping to manage builds for specific enviornments

## Authorization
.Net Core Identity is used. (...or ADB2C) Users are assigned to a Account object.

Authorization is built into all Command related methods via MediatR

## CosmosDB Document Partitioning Strategy
Our strategy is to use **'_docType'** as our partition on an "Unlimited" CosmosDB collection. This project uses the SQL API for document management.

 * Account document _docTypes are named: **"Account"**
 * Platform document _docTypes are named: **"Platform"**
 * Documents belonging to an Account are named: **"Account\<AccountId\>"**
 * Documents for a specific entity type are named: **"\<EntityName\>"**
 * Documents belonging to an entity are named: **"EntityName-\<EntityId\>"**
 * Documents of a particular entity type belonging to a spcific account are named: **"EntityName-Account-\<AccountId\>"**
 * Documents belonging to particular entity type for a specific account are named: **"EntityName-\<EntityId\>-Account-\<AccountId\>"**
 * Partitions that may exceed the 10gb limit should append a date key at the end: **"\<_docType\>-\<YYYYMM\>"**
 

## Recommended Deployment Scenario

![Deploy](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/typical-setup.png)