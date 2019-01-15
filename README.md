# .Net Core Clean Architecture
.Net Core starter project for clean architecture showcasing use of the CQRS pattern, MediatR for cross-cutting concerns, service communications with REST APIs using Swagger, FluentValidation, AutoMapper, CosmosDB for data and Table Storage for logging.

Based on [Jason Taylor's talk on Clean Architecture](https://www.youtube.com/watch?v=_lwCVE_XgqI) with a lot of inspiration from Eric Evans classic book on [Domain-Driven Design](https://www.amazon.com/gp/product/0321125215) and of course the absolutely beutiful work of Jimmy Board and his [MediatR](https://github.com/jbogard/MediatR) project.

This project stresses Domian Driven Design and can be leverged to develop Event Sourced solutions.

![Architecture](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/clean-architecture.png)

# Core
Centralized business logic broken out into **Domain**, **Application** and **Infrastructure** layers. Headless implementation allows you to swap out interfaces between Consoles, Tests, WebApi's and WebApps.

The **Domain Layer** is designed to be shared with non-technical domain experts and includes easy to read domain models/entities and important business logic/processes written out as policies.

The **Application Layer** includes command and query logic, lower-level business logic, view models and works as a bridge between both the domain and infrastructure layers. 

The **Infrastructure Layer** includes  persistence layers, configurations, mediator pipelines, mediator notifications, diagnostics, logging and 3rd party integrations. 

# Services (AKA: CoreServices)
One of many options to wrap and deploy the Core. In this scenario we use a REST API and Webhooks deployable to Linux or Windows as a Docker container to a variety of platforms including:
 * Azure, AWS, Google or any major cloud provider
 * Virtual machines or clusters
 * Azure Web Apps, Amazon EC2 or Google AppEngine
 * Azure Service Fabric
 * Azure Service Fabric Mesh
 * Kubernetes
...or any container orchastrator of your choice.

# Clients
It is recommened that you break clients out into seperate repositiores and build systems so they can be managed by seperate teams. This also allows you to develop the client(s) across your enviornment(s) decoupled from CoreServices.

# Domain Driven Design
A clean archtecture is only as good as the requirements gathering and design process that precedded it. It is important to include non-technical domain experts early and often. This will ensure that the real world problems you are trying model or solve problems for is clearly respresented in the software you are building.

## Core.Services
Main entry point. Wraps the Core libraries and creates a gateway for access by the clients.

DependecyInjection is handled by default .Net Core ServiceProvider. Console and Test entry points are provided in the Utilities folder. API client examples are in their respective folders on the root of the project. 

More details can be found in the [ReadMe](CoreServices/README.md) doc for the CoreServices solution.

**Note:** You can also to develop a more UI centric entry point (Such as a Razor Pages project). This can facilitate building something like an admin portal. This removes the need to build out a seperate web client that needs to autheticate to a REST API for certain scenarios where you may not need a service layer. You may still choose to deploy a seperate set of CoreServices for acces by other clients with only REST endpoints in place. This can also be part of your Razor solution or a seperate project/deployment. These other instances could be focused on only reads, or may only allow a certain subset of commands to run.

 
## CQRS Pattern and Event Sourcing
The Command Query Responsibility Segregation pattern is used for all access to Core. The [MediatR](https://www.nuget.org/packages/MediatR) Nuget package is used for in-process messaging.

### Event Sourcing
 Event sourcing is a data solution that stores every event that occurs in a system in an append only fashion. This is similar to how an accountant adds new information to a ledger, or how a blockchain appends transactions and blocks to it's historical record. From this store of events you can generate aggregates and projections that represent any entity or state from any point in time. The CQRS pattern lends itself well to event sourced solutions and this project should be a good starting point for any project that wants to build an event sourced solution.'

**Note:** Current implementation uses document storage and segregate writes from reads at the persistence layer by updating Azure Search and Redis Cache when commands add, transform or delete entities in the main data stores.
![CQRS-Clean-Architecture](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs-clean-architecture.png)

When implementing event sourcing you will want to seperate your event store (writes) from your snapshots and projections (reads) in a more robust manner by having background tasks and analytical procedures that can leverage the event logs to create an almost unlimited amount of perspectives on your data. When fully implemented the CQRS Pattern along with Event Sourcing allows us to roll back to any state of our application and build up projections and aggregates of any kind from our event logs in an unlimited way. Azure Data Lake and Data Lake Analytics are strong contenders for just such a solution.

![CQRS-Data-Lake](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs-data-lake.png)


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
 * Not contrained to initial use case or domain model.

## MediatR
MediatR allows us to easily create and send Command and Query objects to the correct Command/Query Handlers:

![MediatR](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr.png)

Here is a typical file structure, simplified to focus on a single entity wth only 2 commands and 2 queries:

![CQRS-File-Structure](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/cqrs-file-structure.png)

**Commands:** Commands include properties we expect to have passed in to run the command. They take the place of models that would normally be passed into a function. 

**Queries:**  Similar to Commands, Query objects include properties that the handler will expect to work with when being run.

**Note:** When added to your Dependency Injection Container MediatR will automatically search your assemblies using for **IRequest** and **IRequestHandler** and other MediatR naming conventions. This builds up your library of commands and queries for use throught your project.

**Optimization:** Seperating Commands from Queries allows you to optimize how you access your data store between each access type. You may also choose to have different data stores for reads vs writes. Perhaps Reads will ALWAYS hit a Redis Cache or Search Provider and part of the responsibility of Writes are to ensure these are kept up to date. You may use the same ata store but opt for Entity Framework for your Reads and ADO.NET for your Writes. Or you can go full Event Sourced and build up snapshots, projections and aggregates from your Event Store.

## MediatR Pipeline Behaviors for Cross-Cutting Concerns
MediatR gives you the ability to inject functionality into it's processing pipeline such as pre-request and post-request handlers.  this allow for  on the requests coming in, or post-process the request on the way out. We can define pipelines to help manage cross-cutting concerns such as logging, authorization and caching. 

![Pipelines](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-pipeline.png)
    
	public class NotificationsAndTracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			Trace.WriteLine("Before");
			var response = await next();
			Trace.WriteLine("After");
			return response;
		}
	}

**Note:** You can see an example of pipleline behaviors in the **Core.Infrastructure.Pipeline.NotificationsAndTracingBehavior**, **Core.Infrastructure.Pipeline.PerformanceBehavior** and **Core.Infrastructure.Pipeline.LoggingBehavior** implementations.

**Note:** The **NotificationsAndTracingBehavior** Pipeline will send 'Ping' notifications to 'Pong' subscribers and trace diagnostic messages to your Output window during debugging. 

**Note:** Examples are run on every handler. You can designate pipelines for specific handler types by using if/case statements: **if (request is IQuery)** within your behavior handler or by.

**Note:** Pipelines must be added to your DI container during main entrypoint startup. 

## MediatR Notifications
MediatR allows you to publish a message that can be picked up by any handlers subscribed to it. 

![Notifications](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-notifications.png)

**Note:** You can see an example of notifications used inside of a pipleline behavior here: **Core.Infrastructure.Pipeline.NotificationsAndTracingBehavior** - This example uses the **Core.Infrastructure.Notifications.PingPong.Publisher.Ping** notification and **Core.Infrastructure.Notifications.PingPong.Subscribers.Pong1/2** NotificationHandler(s).

## MediatR Pipeline Behaviors with Notifications
The illustration below showcases how the Ping/Pong Notification Pub/Sub example is used within the TraceBehavior Pipeline/Behavior.

![Pipelines-Notifications](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/mediatr-pipeline-notifications.png)

**Note:** Notifications are a great way to mediate background responsibilities such as adding an object to cache after it is created, or refreshing/removing it after it has changed or been deleted. You can read the response object after a handler is called to determine if the task succeeded/failed, who initiated the task and send the proper notification to the proper handler(s).

## Dependency Injection and CoreConfiguration
Dependency Injection is handled through the default .Net Core Service Provider. An ICoreConfiguration interface is used to encapsulate many applcation related settings. 3rd party service connectors are set up through various interfaces such as IDocumentContext, IStorageContext and IRedisContext.

This is a helper class found in **Core.Common** that allows us to grouop many application, infrastructure and persitence related settings and connectors into a single class within our DI Container.

The **InitializeCoreConfiguration()** should be called in your main entry point prior to adding the fully initialized CoreConfiguration class into your DI Container. 

## Middleware for exception handling and logging
All exceptions should bubble up to the main caller. Projects should enable the middleware for exception handling which allows for logging and handling of all exceptions in one place with the ability to handle specific exception types conditionally.

![Middleware](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/middleware.png)

Exception handling middleware is found here: **Core.Infrastructure.Middleware.ExceptionHandling**

Additional middleware examples can be found in the **Core.Infrastructure.Middleware** root.

Associated extension method allows for registration in **Startup.Configuration()** via **app.UseExceptionHandlerMiddleware();**:

    `public void Configure(IApplicationBuilder app)
	{
		app.UseExceptionHandlerMiddleware();
	}`

## Structured Logging with Serilog
Core libraries are set up to use the global, statically accessible logger from Serilog which must be set up in the main entrypoint(s) and does not require a DI container.

File Sink is commented out and can be replaced with Serilogs vast library of available sinks and diagnostic tooling.

![Serilog-Kestrel](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/serilog-kestrel.png)

For more info on Serilog visit their [website](https://serilog.net/) or [wiki](https://github.com/serilog/serilog/wiki).

It is recommended that you use *structured logging* and send your logs to a log management system. This will allow you greater flexibility to understand and search your diagnostic data. For more on structured logging read the [Serilog wiki](https://github.com/serilog/serilog/wiki/Structured-Data) on the subject.

A list of Serilog sinks: [Provided Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)

Log management systems: [Seq](https://getseq.net/), [PaperTrail](https://papertrailapp.com/) and [Stackify](https://stackify.com/retrace-log-management/)

## Why no Repository Pattern?
I think Jummy Bogard said it best in his contribution to [this article](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design):

> I'm really not a fan of repositories, mainly because they hide the important details of the underlying persistence mechanism. It's why I go for MediatR for commands, too. I can use the full power of the persistence layer, and push all that domain behavior into my aggregate roots. I don't usually want to mock my repositories - I still need to have that integration test with the real thing. Going CQRS meant that we didn't really have a need for repositories any more.

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

Here is an example of how you may want to set up your collection:

![collection-settings](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/collection-settings.png)

### DocumentType Dynamic Constants
To ensure the integrity of the _docType naming convention the **Core.Common.Constants.DocumentTypes** static class should be used when assigning _docTypes to your DocumentModels.

### Integrating Azure Search

CosmosDB has integrated indexing capabilities with Azure Search. 

![azure-search-indexer](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/azure-search-indexer.png)

Note: Once integrated you will need to set up a good interval for scanning your document store that makes sense for your application. It may make sense to include a call to manually run deltas with an API call whenever you run a command that adds, updates or deletes data in your store so that the update is reflected immediatly in your search results.

## OpenAPI and Swagger
CoreServices exposes REST endpoints using OpenAPI Standards. Swagger is used extensively to aid in API documentation, client integration and code generation.

### NSwagStudio
You can use [NSwag Studio](https://github.com/RSuter/NSwag/wiki/NSwagStudio) to generate client code and models. When using with .Net Core you may need to [update your publish configuration](https://github.com/RSuter/NSwag/wiki/Assembly-loading#net-core) to ensure all referenced DLLs are output. 

![NSwag](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/nswag.png)

Generated document describing the endpoints: **http://localhost:<port>/swagger/v1/swagger.json**

The Swagger UI can be found at: **http://localhost:<port>/swagger**

![Swagger-UI](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/swagger-ui.png)

**Note:** On a production application you should create a class library for your Swagger clients to use. These should be versioned and made available as a Nuget package (public or private).

**Note:** Save your NSwag settings! Our are located in the "CoreServices" folder for each REST client as well as in the [Utilities](https://github.com/INNVTV/NetCore-Clean-Architecture/tree/master/Services/Utilities/OpenAPI) folder.

For more details on the OpenAPI implementation used check out the detailed [ReadMe](https://github.com/INNVTV/NetCore-Clean-Architecture/tree/master/Services/Utilities/OpenAPI/README.md) file.

## Webhooks
Simple webhooks to help integrate with 3rd party service providers as well as background tasks/processes.

## ServiceModels
Service models that are used to accept some incoming requests from  client applications in order to avoid exposing MediatR request objects or to perform tasks requeired to create those objects.

In most cases they will be transformed into the appropriate command or query using AutoMapper instance version (Core library uses static). After transformation manual authorization and authentication details or other details not received from clients will be appended to the Command/Query objects. This could be details such as account data partitions or security details derived from the authentication/authorization processes.

**Note** AutoMapper is initialized seperatly within the Srevices project. The Core project will be a compiled DLL or could be a nuget package in the future and it's AutoMapper settings should have no knowledge of those in the layer above.

**Note:** The Core libraries should have no concept of these models as they only serve the purpose of service to client communication.

## ViewModels
View models that are returned from Query methods will include UI related values such as "EditEnabled" and "DeleteEnabled"

## Service-to-service Communication
Examples of clients accessing the service layer are shown in both REST and gRPC flavors.

## Activity Logging

In addition to diagnostics logging from Serilog found in the Performance and Logging Pipleines you should consider implementing application activity logging.

These are used for human readable logs for platform and account admins to view in their respective portals and can be focused on user activity that focuses on the domain and not the infrastructure.

While it is tempting to add this to a Pipeline Behavior - considering the fact that you will want to run authentication/authorization checks (see: authentication/authorization below), manage role allowances and log user details with the activity - this could be better suited to the Service layer where you are closer to the user request and can use this as a gateway to the Commands/Queries in the Application layer below. This also allows you to be more flexible in how you address activity logs as you can check for successes or failures on the Commands/Queries made and decide if certain requests are worth logging. YOu may also want to diferentiate between Account and Platform users and store activities in seperate logs.

Regardless  - command objects include commented out regions should you desire to include user details for activity logging as a pipeline behavior.

## Authentication/Authorization
Authorization is left open. .Net Core Identity or Azure Active Directory (including the B2C variant) should all be considered.

Command objects include commented out regions should you desire to include authorization checks as a pipeline behavior.

## Containerization
Docker is used on all projects/solutions to manage local builds and deploy to multi-enviornment configurations.

## Configuration
We use .Net Cores built in with Docker and Docker compose helping to manage builds for specific enviornments

## AutoMapper (Static and Instance)
AutoMapper **Static** Mappings are configured within the Core.Startup.AutoMapperConfiguration Class.

AutoMapper is also configured seperatly using the **Instance** type within the Services project.

**Note:** This is done because the Core library should have no knowledge of AutoMapper configurations in the layer above it. The Core project will be a compiled DLL or could be a Nuget package in the future. 

## Email
Use the IEmailService interface to implement your email service provider. The default implementation within this project uses SendGrid.


## Recommended Deployment Scenario

![Deploy](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/typical-setup.png)

## Microservices Approach
Each Core can be developed into seperate sub-domains and using a "wrapper" (such as Core.Service) can each be developed and deployed into it's own microservice for a variety of scenarios.

This allows for an even greater seperation of concerns and the ability to scale/evolve each core/service as it's own domain or domain within a domain.

Here is an example of such a soluton using a variety of microservices as well as an admin portal with a Razor View:

![Microservices](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/microservices.png)


# TODO

 * TODO: Pong 2 on success only

 * TODO: Console Cleaner Methods for Debug Methods
 * TODO: Resolve continuation tokens in MediatR
 * TODO: Incorporate Unit of Work?

 * TODO: View Accounts, Details

 * TODO: Add Dependency Creation into Core.Startup

 * TODO: Restore AppSettings to GitHub
