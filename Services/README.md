# Core.Services
Main entry point is the Core.Services WebAPI project. Set this as the "Startup Project" in Visual Studio. VS should detect that it is configured for Docker and give you the ability to debug with Docker. If not you may need to right click on the Core.Services project and choose **Add > Docker Support**.

Core.Services is is the gateway to the application and domain business logic within the Core.Application and Core.Domain projects as well as the other shared libraries that make up the core business logic.

## Utilities/Console
During local debugging you may want to change your main entry point to be the Console app within Utilities. This way you can easily interact with the application layer without having to use an API or RPC client.

## Utilities/Tests
Your testing project(s) should go into Utilites/Tests. There is no predefined project included with this template.


## Deployment
Deployment should be managed through a container registry. Dockerfile for publishing is located in the root of the Core.Services API project.

You can push your containers to an Azure Web App, Service Fabric, Service Fabric Mesh, Kubernetes or any Container Orchestrator of your choice.


#The Service Gateway Layers

## REST APIs
 Located within "Controllers" folder
 
     REST Endpoints: /api
 
## RPC
gRPC implementation. Used by background workers and custodians. Custodial and Platform calls are initited through here.

    gRPC Endpoints: /rpc

## WebHooks
APIs for integration with 3rd party systems such as Stripe or Event Grid.

    Webhook Endpoints: /webhooks

# Configuration
Local and debug settings are stored within the appsettings.json file in the Core.Services API project.

When pushing to production these can/should be overriden by Application Settings on the hosted server by replacing the : syntax for navigating appsettings.json sections with double underscore: __

 > Example: **Application:Name:Default** == **Application__Name__Default**

You can also link the value to an Azure KeyVault instance for full encryption:

![AppSettingsVault](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/CoreServices/_docs/imgs/app-settings-vault.png)
