### Swagger and NSwagStudio
Use [NSwag Studio](https://github.com/RSuter/NSwag/wiki/NSwagStudio) to generate client code and models. When using with .Net Core you may need to [update your publish configuration](https://github.com/RSuter/NSwag/wiki/Assembly-loading#net-core) to ensure all referenced DLLs are output. 

Generated document describing the endpoints: **http://localhost:<port>/swagger/v1/swagger.json**

The Swagger UI can be found at: **http://localhost:<port>/swagger**

**Note:** On a production application you should create a class library for your Swagger clients to use. These should be versioned and made available as a Nuget package (public or private).

**Note:** Save your NSwag settings! **nswag.json** are the settings used to generate the client and contracts found in this folder.