 * TODO: Complete and Test Validation in Commands/Queries (Make part of MediatR)
 * TODO: Add Exception Response in Commands/Queries
 * TODO: Add Cross-Cutting (ActivityLog, ExceptionLog, ErrorLog, Authorization) in Commands/Queries
 * TODO: Finalize Automapper
 * TODO: Restore AppSettings to GitHub

# Core.Services
Main entry point is the Core.Services WebAPI project. Set this as the "Startup Project" in Visual Studio. VS should detect that it is configured for Docker and give you the ability to debug with Docker. If not you may need to right click on the Core.Services project and choose **Add > Docker Support**.

Core.Services is is the gateway to the application and domain business logic within the Core.Application and Core.Domain projects as well as the other shared libraries that make up the core business logic.

## Utilities/Console
During local debugging you may want to change your main entry point to be the Console app within Utilities. This way you can easily interact with the application layer without having to use an API or RPC client.

## Utilities/Tests
Your testing project(s) should go into Utilites/Tests. There is no predefined project included with this template.

## Domain Driven Design
A clean archtecture is only as good as the requirements gathering and design process that precedded it. It is important to include non-technical domain experts early and often. This will ensure that the real world problems you are trying model or solve problems for is clearly respresented in the software you are building.

## CQRS Pattern
The Command Query Responsibility Segregation pattern is used for all access to the business logic within the Core.Application project. This implementation shares the same persistence layer, however the class seperation easily allows you to use a different data store for your queries.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html

## Event Sourcing
 Event sourcing is a data solution that stores every event that occurs in a system in an append only fashion. This is similar to how an accountant adds new information to a ledger, or how a blockchain appends transactions and blocks to it's historical record. From this store of events you can generate aggregates and projections that represent any entity or state from any point in time. The CQRS pattern lends itself well to event sourced solutions and this project should be a good starting point for any project that wants to build an event sourced solution.'

## Mediator Design Pattern
Mediator Design Pattern is implemented using the MediatR library:

    dotnet add package MediatR

## Cross-Cutting Concerns
Cross-cutting concerns such as logging, authorization and caching are handled by Mediatr

## Logging
Logging is handled by the ICoreLogging interface. Uses partitioned logging inside of Azure Table Storage. We log by Time, Activity and Account. This implementation can be used as an example for adding additional dimensions.

Logging is built into all Command related methods via MediatR

## Deployment
Deployment should be managed through a container registry. Dockerfile for publishing is located in the root of the Core.Services API project.

You can push your containers to an Azure Web App, Service Fabric, Service Fabric Mesh, Kubernetes or any Container Orchestrator of your choice.

## The Application Layer

 Core.Application Project - This is the business logic layer. Many infrastructure specific models are represented with this layer - but will be transfomed into domain models when passed back to consumers.
 
## The Domain Layer

Core.Domain Project - The classes represented here are a result of collaboration with the non-technical domain experts on the project. Any application models will likely be trasformed from or to the domain models represented in this project in order to better represent the real world model this software strives to represent.


#The Service Gateway Layers

## REST APIs
 Located within "Controllers" folder
 
     REST Endpoints: /api
 
## RPC
gRPC implementation. Used by background workers and custodians. Custodial and Platform calls are initited through here.

    gRPC Endpoints: /rpc

## WebHooks
APIs for integration with 3rd party systems such as Stripe or Event Grid.

    Webhoo Endpoints: /webhooks

# Configuration
Local and debug settings are stored within the appsettings.json file in the Core.Services API project.

When pushing to production these can/should be overriden by Application Settings on the hosted server by replacing the : syntax for navigating appsettings.json sections with double underscore: __

 > Example: **Application:Name:Default** == **Application__Name__Default**

You can also link the value to an Azure KeyVault instance for full encryption:

![AppSettingsVault](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/CoreServices/_docs/imgs/app-settings-vault.png)
