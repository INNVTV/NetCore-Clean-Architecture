# Clients
When developing a new project these clients should be seperated into their own repositories, build and release systems.

## OpenAPI/Swagger
All REST clients in this leverage generated client code using OpenAPI standards.

### NSwagStudio
Use [NSwag Studio](https://github.com/RSuter/NSwag/wiki/NSwagStudio) to generate client code and models. When using with .Net Core you may need to [update your publish configuration](https://github.com/RSuter/NSwag/wiki/Assembly-loading#net-core) to ensure all referenced DLLs are output. 

## gRPC
All gRPC clients in this example leverage generated client code using gRPC.

## Webhooks
All Webhook clients use HttpClient to access CoreServices webhook endpoints.
