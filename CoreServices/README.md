# Core.Services
Main entry point is the Core.Services API project. Set this as the "Startup Project" in Visual Studio. VS should detect that it is configured for Docker and give you the ability to debug with Docker. If not you may need to right click on the Core.Services project and choose **Add > Docker Support**.

Core.Services is is the gateway to the application and domain business logic within the Core.Application and Core.Domain projects as well as the other shared libraries that make up the core business logic.

# Utilities/Console
During local debugging you may want to change your main entry point to be the Console app within Utilities. This way you can easily interact with the application layer without having to use an API or RPC client.

# Utilities/Tests
Your testing project(s) should go into Utilites/Tests. There is no predefined project included with this template.


## CQRS
The Command Query Responsibility Segregation pattern is used for all access to Core.Domain through the Core.Application project as a gateway.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html

## Cross-Cutting Concerns
Cross-cutting concerns such as logging, authorization and caching are handled by Mediatr

## MediatR
xxxx

## Logging
Logging is handled by Table Storage. We log by Time, Activity, Account and this can be used as an example for adding additional dimensions.

Logging is built into all Command related methods via MediatR

## Deployment
Deployment should be managed through a container registry. Dockerfile for publishing is located in the root of the Core.Services API project.

You can push your containers to an Azure Web App, Service Fabric, Service Fabric Mesh, Kubernetes or any Container Orchastrator of your choice.

 # The Application Layer
 Core.Application Project
 
 # The Services Layer
 Core.Services Project
 
 ## Event Sourcing
xxxx



 ## REST APIs
 Located within "Controllers" folder
 
     REST Endpoints: /api
 
 ## RPC
gRPC implementation. Used by background workers and custodians. Custodial and Platform calls are initited through here.

    gRPC Endpoints: /rpc

## WebHooks
APIs for integration with 3rd party systems such as Stripe or Event Grid.

    Webhoo Endpoints: /webhooks

## Configuration
Local and debug settings are stored within the appsettings.json file in the Core.Services API project. When pushing to production these should be overriden by Application Settings on the hosted server and linked to an Azure KeyVault instance for the best encryption security.
