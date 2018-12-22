# .Net Core Clean Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, micro-service communications with both REST and gRPC endpoints, FluentValidation, CosmosDB for data and Table Storage for logging.

Based on [Jason Taylor's talk on Clean Architecture](https://www.youtube.com/watch?v=_lwCVE_XgqI) with a lot of inspiration from Eric Evans book on [Domain-Driven Design](https://www.amazon.com/gp/product/0321125215)

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
UI client(s) that connect to CoreServices via REST APIs.

**Note:** It is recommened that you break WebClients out into seperate repositiores and build systems so they can be managed by seperate teams. This also allows you to develope the clients across the enviornments that CoreServices may be running on.

# TaskClients
Background tasks hosted as workers that connect to CoreServices via gRPC.

**Note:** It is recommened that you break TaskClients out into seperate repositiores and build systems so they can be managed by seperate teams. This also allows you to develope the clients across the enviornments that CoreServices may be running on.

## Core.Services
Main entry point. Headless implementation allows you to swap out interfaces between Consoles, Tests, WebApi's and WebApps. Includes Core.Common, Core.Application and Core.Domain Projects and wraps them into a gateway for access by the clients.

DependecyInjection is handled by default .Net Core ServiceProvider. Console and Test entry points are provided in the Utilities folder. API and gRPC client examples are in their respective folders on the root of the project. 

More details can be found in the [ReadMe](CoreServices/README.md) doc for the CoreServices solution.

**Note:** You can also to develop a more UI centric entry point (Such as a Razor Pages project). This can facilitate building something like an admin portal. This removes the need to build out a seperate web client that needs to autheticate to a REST API for certain scenarios where you may not need a service layer. You may still choose to deploy a seperate set of CoreServices for acces by other clients with only REST endpoints in place. This can also be part of your Razor solution or a seperate project/deployment. These other instances could be focused on only reads, or may only allow a certain subset of commands to run.

 
## CQRS Pattern and Event Sourcing
The Command Query Responsibility Segregation pattern is used for all access to Core. The [MediatR](https://www.nuget.org/packages/MediatR) Nuget package is used for in-process messaging.

Event Sourcing is not fully implemented and allows you to develop using your Event Store of choice such as: EventStore, StreamStone or your own custom solution.

**Note:** Current implementation uses document storage and does not fully segregate writes from reads at the persistence layer - only within the application layer. When implementing event sourcing you will want to seperate your event store (writes) from your snapshots and projections (reads) as shown here:
![CQRS-Reads-Writes](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs-reads-writes.png)

When fully implemented the CQRS Pattern along with Event Sourcing allows us to roll back to any state of our application and build up projections and aggregates of any kind from our events.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html

For more on Event Sourcing: https://martinfowler.com/eaaDev/EventSourcing.html

### CQRS Benefits
 * Clean seperation of Read/Write concerns.
 * Ability to scale/optimize Reads separately from Writes.
 * Ability to use seperate data stores between reads/writes (For example: SQL for Writes and Redis for Reads)
 * Thin controllers. 
 * Ability to use the Event Sourcing pattern.

### Event Sourcing Benefits
 * Break out of the constrainst of thinking "relationally" which locks you into strict domain models.
 * Focus on actions and events, rather than entities and relationships.
 * Immutable tranasctional history of every event that occured in the system.
 * Ability to rewind/fast-forward to any state/time. (Great for debugging and auditing).
 * Future proof your data with unlimited capability to develop future projections from your Event Store (AKA Fact Logs).
 * Nit contrained to initial use case or domain model.

## MediatR
MediatR allows us to easily create and send Command and Query objects to the correct Command/Query Handlers:

![MediatR](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr.png)

Here is a typical file structure, simplified to focus on a single entity wth only 2 commands and 2 queries:

![CQRS-File-Structure](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs-file-structure.png)

**Commands:** Commands include properties we expect to have passed in to run the command. They take the place of models that would normally be passed into a function. 

[Command Class]

[Handler Class]

**Queries:**  Similar to Commands, Query objects include properties that the handler will expect to work with when being run.


[Query Class]

[Handler Class]

**Note:** When added to your Dependency Injection Container MediatR will automatically search your assemblies using for **IRequest** and **IRequestHandler** and other MediatR "conventions" and will build up your library of commands and queries for use throught your project.


**Optimaization:** Seperating Commands from Queries allows you to optimize how you access your data store between each access type. You may also choose to have different data stores for reads vs writes. Perhaps Reads will ALWAYS hit a Redis Cache or Search Provider and part of the responsibility of Writes are to ensure these are kept up to date. You may use the same ata store but opt for Entity Framework for your Reads and ADO.NET for your Writes. Or you can go full Event Sourced and build up snapshots, projections and aggregates from your Event Store.

## MediatR Pipelines for Cross-Cutting Concerns
MediatR gives you the ability to create pipelines to help manage cross-cutting concerns such as logging, authorization and caching. This example showcasing a logging feature.

[Exampl with images]

## MediatR Notifications
MediatR

## ViewModels
View models that are returned from Query methods will include UI related values such as "EditEnabled" and "DeleteEnabled"

## Service-to-service Communication
Examples of clients accessing the service layer are shown in both REST and gRPC flavors.

## Logging

In addition to Event Source logging we also log activities via ICoreActivityLogger. These are used for human readable logs for platform and account admins to view in their respective portals.

## Containerization
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

## Microservices Approach
Each CoreService can be developed into it's own microservice and composed for a variety of scenarios.

This allows for an even greater seperation of concerns and the ability to scale/evolve each service as it's own domain.

Here is an example of such a soluton using a variety of microservices as well as an admin portal with a Razor View:

![Microservices](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/microservices.png)