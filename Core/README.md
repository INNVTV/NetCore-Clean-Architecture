## The Application Layer

 Core.Application Project - This is the business logic layer. Many infrastructure specific models are represented with this layer - but will be transfomed into domain models when passed back to consumers.
 
## The Domain Layer

Core.Domain Project - The classes represented here are a result of collaboration with the non-technical domain experts on the project. Any application models will likely be trasformed from or to the domain models represented in this project in order to better represent the real world model this software strives to represent.

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


