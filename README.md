# Restaurants API Backend

This is a Backend API for a Restaurant Management System in .NET Framework using CQRS pattern and DDD.

The API can be accessed at the following URLs:

- [Staging Environment](restaurants-api-dev-dhbtgce0bnhyfyfq.uaenorth-01.azurewebsites.net)

- [Production Environment](abhiroopsantra-restaurants-api-prod.azurewebsites.net)

## Tech Stack

- **Language**: C#
- **Framework**: .NET LTS
- **Database**: SQL Server / Azure SQL
- **ORM**: Entity Framework Core
- **Validation**: FluentValidation
- **Logging**: Serilog, Azure Application Insights
- **API Documentation**: Swagger
- **Testing**: xUnit, Moq
- **CI/CD**: GitHub Actions
- **Deployment**: Azure App Service
- **Design Patterns**: CQRS, Mediator, Repository, Domain-Driven Design

## Project Structure

### Restaurants.API

This project contains the API layer of the Restaurants application. It includes controllers, middlewares, and extensions for setting up the API.

- **Controllers**: Handles HTTP requests and responses.
- **Extensions**: Contains extension methods for configuring services.
- **Middlewares**: Custom middleware for handling errors and request timing.
- **Program.cs**: Entry point of the API application.

### Restaurants.Application

This project contains the application layer, which includes business logic and application services.

### Restaurants.Domain

This project contains the domain layer, which includes domain entities and business rules.

### Restaurants.Infrastructure

This project contains the infrastructure layer, which includes data access and external service integrations.

### Tests

This folder contains unit and integration tests for the various projects.

- **Restaurants.API.Tests**: Tests for the API layer.
- **Restaurants.Application.Tests**: Tests for the application layer.
- **Restaurants.Domain.Tests**: Tests for the domain layer.
- **Restaurants.Infrastructure.Tests**: Tests for the infrastructure layer.

## Getting Started

To build and run the solution, follow these steps:

1. Clone the repository.
2. Open the solution file `Restaurants.sln` in your favorite IDE.
3. Build the solution.
4. Create a .env file in the `src/Restaurants.API` directory (example provided in .env.example)
5. Run the `Restaurants.API` project.
6. (Optional) Run the unit tests with `dotnet test -e APPLICATIONINSIGHTS_CONNECTION_STRING="<your-connection-string>"`

## Configuration

Configuration files for the API project are located in the `src/Restaurants.API/` directory:

- `appsettings.Development.json`: Development-specific configuration settings.

## Logging

The solution uses Serilog for logging. Configuration for Serilog can be found in the `Program.cs` file of the `Restaurants.API` project. The logs are generated in the console, in log files within the `src/Restaurants.API/Logs` directory, and in Application Insights (connection string to be defined in .env file).

## Swagger

Swagger is used for API documentation. It is configured in the `AddPresentation` method in the `WebApplicationBuilderExtensions` class located in `src/Restaurants.API/Extensions/WebApplicationBuilderExtensions.cs`. The swagger UI can be accessed at `/swagger` endpoint.

## Middleware

Custom middleware for error handling and request timing is registered in the `AddPresentation` method and used in the `Program.cs` file of the `Restaurants.API` project.

## Seeding

The `Restaurants.Infrastructure` project includes a seeder for initializing the database with sample data. It is invoked in the `Program.cs` file of the `Restaurants.API` project.

## License

This project is licensed under the MIT License.
