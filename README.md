# Customers Microservice

Application developed thinking about the ecosystem of a microservice using DDD (Domain Driven Design), DI (Dependency Injection) native ASP.NET Core, Repository Pattern, decoupling of services and API configurations using extension classes, unit tests, native .NET logging and integration with AWS Secret Manager for managing users with access to the microservice, all containerized using Docker.

## Interface

The interface used for the API is [Swagger](https://swagger.io/), simple, practical and very intuitive.
![image](https://github.com/LuizFilipeDev/customers-microservice/assets/74942532/79f63168-da8e-4106-b2df-96cf260d54c4)

## What are the functions?

The functionalities are basic characteristics for a CRUD using memory data, since the objective is the architecture and the means used to create the microservice.

## Architecture

### DDD

The DDD used was based on the architecture recommended by [Microsoft](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice) community itself, with some adjustments to make the architecture a little more dynamic for different applications.
