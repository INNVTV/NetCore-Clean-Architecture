# The Domain Layer

This project contains all entities, enums, types and logic specific to the domain and not related to the application or persistence layer.

## Domain Classes

The classes represented here are a result of collaboration with the non-technical domain experts on the project. Any application models will likely be trasformed from or to the domain models represented in this project in order to better represent the real world model this software strives to represent.

## Policies/Strategies

Using a Layered architecture, the strategy pattern (also known as the policy pattern) is a behavioral software design pattern that enables selecting an algorithm at runtime. Instead of implementing a single algorithm directly, code receives run-time instructions as to which in a family of algorithms to use.

In DDD this pulls important business polices/artifacts outside of deep application logic and into a format easily understood by non-technical domain experts. 

This also gives you a single place to find and manage important polices that are being enforced upon the software.

## Model Transformations

Model transformations are facilitated through the AutoMapper Nuget Package

    dotnet add package AutoMapper