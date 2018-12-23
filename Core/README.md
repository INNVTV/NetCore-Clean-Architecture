# The Application Layer

 Core.Application Project - This is the business logic layer. Many infrastructure specific models are represented with this layer - but will be transfomed into domain models when passed back to consumers.
 

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

Core.Domain Project - The classes represented here are a result of collaboration with the non-technical domain experts on the project. Any application models will likely be trasformed from or to the domain models represented in this project in order to better represent the real world model this software strives to represent.

### Entities

Domain entities are the clean versions of our models without the added properties of our infratructure overcomplicating them. This layered apporach makes it easier to have non-technical domain experts weigh in on their design.

### Policies (Strategy Pattern)

Domain policies are broken out using the strategy pattern so they can be easily be shared in collaboration with non technical domain experts.


