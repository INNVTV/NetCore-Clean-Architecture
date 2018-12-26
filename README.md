# .Net Core Clean Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, service communications with both REST and gRPC endpoints, FluentValidation, AutoMapper, CosmosDB for data and Table Storage for logging.

Based on [Jason Taylor's talk on Clean Architecture](https://www.youtube.com/watch?v=_lwCVE_XgqI) with a lot of inspiration from Eric Evans book on [Domain-Driven Design](https://www.amazon.com/gp/product/0321125215)

This project stresses Domian Driven Design and can be leverged to develop Event Sourced solutions.

![Architecture](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/clean-architecture.png)

# Core
Centralized business logic broken out into **Application** and **Domain** layers.

The **Domain Layer** is designed to be shared with non-technical domain experts and includes easy to read domain entities and important business processes written out as policies.

The **Application Layer** includes infrastructure implementation, persistence models, view models, logging, authorization and low level business logic. 

# CoreServices
One of many options to wrap and deploy the Core. In this scenario we use a REST API and gRPC endpoints deployable to Linux or Windows as a Docker container to a variety of platforms including:
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

# Domain Driven Design
A clean archtecture is only as good as the requirements gathering and design process that precedded it. It is important to include non-technical domain experts early and often. This will ensure that the real world problems you are trying model or solve problems for is clearly respresented in the software you are building.

## Core.Services
Main entry point. Headless implementation allows you to swap out interfaces between Consoles, Tests, WebApi's and WebApps. Includes Core.Common, Core.Application and Core.Domain Projects and wraps them into a gateway for access by the clients.

DependecyInjection is handled by default .Net Core ServiceProvider. Console and Test entry points are provided in the Utilities folder. API and gRPC client examples are in their respective folders on the root of the project. 

More details can be found in the [ReadMe](CoreServices/README.md) doc for the CoreServices solution.

**Note:** You can also to develop a more UI centric entry point (Such as a Razor Pages project). This can facilitate building something like an admin portal. This removes the need to build out a seperate web client that needs to autheticate to a REST API for certain scenarios where you may not need a service layer. You may still choose to deploy a seperate set of CoreServices for acces by other clients with only REST endpoints in place. This can also be part of your Razor solution or a seperate project/deployment. These other instances could be focused on only reads, or may only allow a certain subset of commands to run.

 
## CQRS Pattern and Event Sourcing
The Command Query Responsibility Segregation pattern is used for all access to Core. The [MediatR](https://www.nuget.org/packages/MediatR) Nuget package is used for in-process messaging.

### Event Sourcing
 Event sourcing is a data solution that stores every event that occurs in a system in an append only fashion. This is similar to how an accountant adds new information to a ledger, or how a blockchain appends transactions and blocks to it's historical record. From this store of events you can generate aggregates and projections that represent any entity or state from any point in time. The CQRS pattern lends itself well to event sourced solutions and this project should be a good starting point for any project that wants to build an event sourced solution.'

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

## MediatR Pipeline Behaviors for Cross-Cutting Concerns
MediatR gives you the ability to inject functionality into it's processing pipeline such as pre-request and post-request handlers.  this allow for  on the requests coming in, or post-process the request on the way out. We can define pipelines to help manage cross-cutting concerns such as logging, authorization and caching. 

![Pipelines](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-pipeline.png)


## MediatR Notifications
MediatR allows you to publish a message that can be picked up by any handlers subscribed to it. 

![Notifications](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-notifications.png)

## MediatR Pipeline Behaviors with Notifications
![Pipelines-Notifications](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-pipeline-notifications.png)




## Dependency Injection and CoreConfiguration
Dependency Injection is handled through the default .Net Core Service Provider. An ICoreConfiguration interface is used to encapsulate many applcation related settings. 3rd party service connectors are set up through various interfaces such as IDocumentContext, IStorageContext and IRedisContext.

This is a helper class found in **Core.Common** that allows us to grouop many application, infrastructure and persitence related settings and connectors into a single class within our DI Container.

The **InitializeCoreConfiguration()** should be called in your main entry point prior to adding the fully initialized CoreConfiguration class into your DI Container. 

## Why no Repository Pattern?
I think Jummy Bogard said it best in his contribution to [this article](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design):

> I'm really not a fan of repositories, mainly because they hide the important details of the underlying persistence mechanism. It's why I go for MediatR for commands, too. I can use the full power of the persistence layer, and push all that domain behavior into my aggregate roots. I don't usually want to mock my repositories - I still need to have that integration test with the real thing. Going CQRS meant that we didn't really have a need for repositories any more.

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

## AutoMapper
AutoMapper Mappings are configured within the Core.Startup.AutoMapperConfiguration Class

## Email
Use the IEmailService interface to implement your email service provider. The default implementation within this project uses SendGrid.

## CosmosDB Document Partitioning Strategy
Our strategy is to use **'_docType'** as our partition on the CosmosDB collection. This project uses the SQL API for document management.

**Here is a recommended ParitionKey naming convention:**

 * Account document _docTypes are named: **"Account"**
 * Platform document _docTypes are named: **"Platform"**
 * Documents belonging to an Account are named: **"Account\<AccountId\>"**
 * Documents for a specific entity type are named: **"\<EntityName\>"**
 * Documents belonging to an entity are named: **"EntityName-\<EntityId\>"**
 * Documents of a particular entity type belonging to a spcific account are named: **"EntityName-Account-\<AccountId\>"**
 * Documents belonging to particular entity type for a specific account are named: **"EntityName-\<EntityId\>-Account-\<AccountId\>"**
 * Partitions that may exceed the 10gb limit should append a date key at the end: **"\<_docType\>-\<YYYYMM\>"**
 
### NameKey
Most entities will have a "NameKey" derived from the name of the entity. This is a pretty version of the entity name that serves as both a unique id as well as an index name for routing.

### Unique Key
CosmosDB allows you to create a "UniqueKey" that adds a layer of data integrity to your collection. This will ensure that this property is not duplicated in any documents within the same partition. "NameKey" would be a good candidate for this extra layer of integrity.

### DocumentType Dynamic Constants
To ensure the integrity of the _docType naming convention the **Core.Common.Constants.DocumentTypes** static class should be used when assigning _docTypes to your DocumentModels.

## Recommended Deployment Scenario

![Deploy](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/typical-setup.png)

## Microservices Approach
Each Core can be developed into seperate sub-domains and using a "wrapper" (such as Core.Service) can each be developed and deployed into it's own microservice for a variety of scenarios.

This allows for an even greater seperation of concerns and the ability to scale/evolve each core/service as it's own domain or domain within a domain.

Here is an example of such a soluton using a variety of microservices as well as an admin portal with a Razor View:

![Microservices](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/microservices.png)


 * TODO: Enable Notification with many subscribers.
 * TODO: Logging using Notification Services or Pre/Post Pipelines?
 * TODO: Use Nitifications with Caching? (To clear cache)
 * TODO: Authorization using Pre-Post Pipelines (with logging?)
 * TODO: Notification and Pipeline Notes in ReadMe

 * TODO: Add Cross-Cutting (ActivityLog, ExceptionLog, ErrorLog, Authorization) in Commands/Queries

 * TODO: Console Cleaner Methods for Debug Methods
 * TODO: Resolve continuation tokens in MediatR
 * TODO: Incorporate Unit of Work?
 * TODO: Redis on Queries (+ Clear Cache On Updates)

 * TODO: View Accounts, Details, Logs
 * TODO: Identity, Roles, Users Creation, Authorization, Logs

 * TODO: Make Validation part of MediatR/Startup?
 * TODO: Add Dependency Creation into Core.Startup
 * TODO: Add Simple Entity to Account Object of Type
 
 * TODO: Restore AppSettings to GitHub
