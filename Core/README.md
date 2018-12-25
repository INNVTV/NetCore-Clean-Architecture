# The Application Layer

 **Core.Application:** This is the business logic layer. Many infrastructure specific models are represented with this layer - but will be transfomed into domain models when passed back to consumers.
 

### CQRS Pattern
The Command Query Responsibility Segregation pattern is used for all access to the business logic within the Core.Application project. This implementation shares the same persistence layer, however the class seperation easily allows you to use a different data store for your queries.

For more on the CQRS pattern: https://martinfowler.com/bliki/CQRS.html


### Mediator Design Pattern
Mediator Design Pattern is implemented using the MediatR library:

    dotnet add package MediatR

### Cross-Cutting Concerns
Cross-cutting concerns such as logging, authorization and caching are handled by Mediatr

### Logging
Logging is handled by the ICoreLogging interface. Uses partitioned logging inside of Azure Table Storage. We log by Time, Activity and Account. This implementation can be used as an example for adding additional dimensions.

Logging is built into all Command related methods via MediatR


# The Domain Layer

**Core.Domain:** The classes represented here are a result of collaboration with the non-technical domain experts on the project. Any application models will likely be trasformed from or to the domain models represented in this project in order to better represent the real world model this software strives to represent.

### Entities

Domain entities are the clean versions of our models without the added properties of our infratructure overcomplicating them. This layered apporach makes it easier to have non-technical domain experts weigh in on their design.

### Policies (Strategy Pattern)

Domain policies are broken out using the strategy pattern so they can be easily be shared in collaboration with non technical domain experts.

# The Infrastructure Layer

**Core.Infrastructure:** The classes represented here include hosting, configuration, persistence services, data storage and other 3rd party integrations.

### Dependency Injection, CoreConfiguration & Contexts
An ICoreConfiguration interface is used to encapsulate many applcation related settings. 3rd party service connectors are set up through various interfaces such as IDocumentContext, IStorageContext and IRedisContext.


# Common 

**Core.Common:** Encapsulates many common settings, transformations, base classes, interfaces and logging used within the application domain or shared with a consumer of the Core project.


# Startup

**Core.Startup:** Set of routines that must be called by consumers of Core in order to initialize all configurations. We do this to simplify startup on services/clients. This avoids having to copy/paste startup routines to various Main and Startup methods. They are all encapsulated here and can run with a single line of code from Startup or Main.

**Note:** Startup is it's own library to avoid circular reference issues.

### AutoMapper
AutoMapper Mappings are configured within the Core.Startup.AutoMapperConfiguration Class
