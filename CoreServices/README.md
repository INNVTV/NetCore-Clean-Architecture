# CoreServices
Main entry point is the Core.Services API project. Set this as the "Startup Project" in Visual Studio. VS should detect that it is configured for Docker and give you the ability to debug with Docker. If not you may need to right click on the Core.Services project and choose **Add > Docker Support** This is the gateway to the application and domain business logic within the Core.Application and Core.Domain projects as well as the other shared libraries that make up the core business logic.

## Cross-Cutting Concerns
Cross-cutting concerns such as logging, authorization and caching are handled by Mediatr

## CQRS
The Command Query Responsibility Segregation pattern is used for all access to Core.Domain through the Core.Application project as a gateway.

## Deployment
Deployment should be managed through a container registry. Dockerfile for publishing is located in the root of the Core.Services API project.

You can push your containers to an Azure Web App, Service Fabric, Service Fabric Mesh, Kubernetes or any Container Orchastrator of your choice.

## Configuration
Local and debug settings are stored within the appsettings.json file in the Core.Services API project. When pushing to production these should be overriden by Application Settings on the hosted server and linked to an Azure KeyVault instance for the best encryption security.
