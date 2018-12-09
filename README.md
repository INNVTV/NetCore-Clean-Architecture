# NetCore-Clean-Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with both REST and gRPC endpoints, FluentValidation, CosmosDB for data and Table Storage for logging.

Based on Jason Taylors talk on "Clean Architecture": https://www.youtube.com/watch?v=_lwCVE_XgqI

![Architecture](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/clean-architecture.png)

# CoreServices
Centralized business logic. Visual Studio Solution Project. Deployable to Linux or Windows as a Docker contianer to a variety of platforms including:
 * Virtual machines or clusters
 * Azure Web Apps
 * Service Fabric
 * Service Fabric Mesh
 * Kubernetes
...or any container orchastrator of your choice.

# WebClients
UI client(s) that connect to CoreServices via REST API. Visual Studio Code projects.

# TaskClients
Background tasks hosted as workers that connect to CoreServices via gRPC. Visual Studio Code projects.

# The Domain layer
 Core.Domain Project
 
### CQRS
The Command Query Responsibility Segregation pattern is used for all access to Core.Domain through the Core.Application project as a gateway.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html

![CQRS](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/cqrs.png)




## ViewModels
View models that are returned from Query methods will include UI related values such as "canDelete" and "canEdit"

## Service-to-service Communication
Examples of clients accessing the service layer are shown in both REST and gRPC flavors.

## Containerization
Docker is used on all projects/solutions to manage local builds and deploy to multi-enviornment configurations.

## Configuratin
We use .Net Cores built in with Docker and Docker compose helping to manage builds for specific enviornments



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
 

## Recommended Deployment Scenario

![Deploy](https://github.com/INNVTV/NetCore-CQRS/blob/master/_docs/imgs/typical-setup.png)