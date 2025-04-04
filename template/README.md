[Back to README](../README.md)

## Overview

This project is a Full Stack Angular developer evaluation for B3. The goal is to demonstrate skills in software development, including creating a robust, efficient, and maintainable web application using modern technologies.

### Project Objectives

- **Frontend**: Develop an interactive and responsive user interface using Angular.
- **Backend**: Implement a robust and secure API using .NET Core.
- **Integration**: Use RabbitMQ for asynchronous communication between services.
- **Persistence**: Manage data using Entity Framework Core.
- **Testing**: Ensure code quality with unit and integration tests.
- **Containerization**: Use Docker to ensure application consistency and portability.

### Technologies Used

- **Frontend**: Angular
- **Backend**: .NET Core
- **Messaging**: RabbitMQ
- **Object Mapping**: AutoMapper
- **Mediator**: MediatR
- **Database**: Entity Framework Core
- **Testing**: Faker, NSubstitute
- **Containerization**: Docker

### Project Structure

- **Frontend**: Contains the Angular application, responsible for the user interface.
- **Backend**: Contains the .NET Core API, responsible for business logic and database communication.
- **Messaging**: Configuration and use of RabbitMQ for communication between services.
- **Testing**: Implementation of unit and integration tests to ensure code quality.
- **Containerization**: Docker configuration to facilitate development and deployment of the application.

### How to Run the Project

1. **Clone the repository**: git clone https://github.com/thiberioazevedo/CDB.git

2. **Open solution**: DeveloperEvaluation.sln

3. **Navigate to the source directory**: cd src

4. **Start the Docker containers**: docker-compose up

5. **Apply the database migrations**: dotnet ef database update -v --project DeveloperEvaluation.ORM --startup-project DeveloperEvaluation.WebApi

6. **Access the application**:
    - Frontend: `http://localhost:4200`
    - Backend:  `http://localhost:5000`
	- Documenting API: `http://localhost:5119/swagger` 
	Swagger streamlines API development by documenting, testing, and sharing RESTful APIs interactively and efficiently	
	
7. **Register a new user**

8. **Log in with the created user**

9. **Access the CDB menu and perform the desired operations (insert, delete, list, get)**
    
## Frameworks
Our frameworks are the building blocks that enable us to create robust, efficient, and maintainable software solutions. They have been carefully selected to complement our tech stack and address specific development challenges we face in our projects.

These frameworks enhance our development process by providing tried-and-tested solutions to common problems, allowing our team to focus on building unique features and business logic. Each framework has been chosen for its ability to integrate seamlessly with our tech stack, its community support, and its alignment with our development principles.

We use the following frameworks in this project:

Backend:
- **Mediator**: A behavioral design pattern that helps reduce chaotic dependencies between objects. It allows loose coupling by encapsulating object interaction.
  - Git: https://github.com/jbogard/MediatR
- **Automapper**: A convention-based object-object mapper that simplifies the process of mapping one object to another.
  - Git: https://github.com/AutoMapper/AutoMapper
- **RabbitMQ**: A message broker that enables applications to communicate with each other and exchange information through messages.
•	Git: https://github.com/rabbitmq/rabbitmq-dotnet-client

Testing:
- **Faker**: A library for generating fake data for testing purposes, allowing for more realistic and diverse test scenarios.
  - Git: https://github.com/bchavez/Bogus
- **NSubstitute**: A friendly substitute for .NET mocking libraries, used for creating test doubles in unit testing.
  - Git: https://github.com/nsubstitute/NSubstitute

Database:
- **EF Core**: Entity Framework Core, a lightweight, extensible, and cross-platform version of Entity Framework, used for data access and object-relational mapping.
  - Git: https://github.com/dotnet/efcore

Containerization:
- **Docker**: A platform for developing, shipping, and running applications in containers. Docker enables us to package applications and their dependencies into a standardized unit for software development, ensuring consistency across multiple environments.
  - Git: https://github.com/docker/docker

### Authentication

#### POST /auth/login
- Description: Authenticate a user
- Request Body:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- Response: 
  ```json
  {
    "token": "string"
  }
  ```

## Project Structure

The project should be structured as follows:

```
root
├── src/
├── tests/
└── README.md
```
## Tech Stack
The Tech Stack is a set of technologies we use to build our Ambev products, which is why we can consider it the heart of our products.

All versions of languages, frameworks, and tools have been carefully chosen based on a predefined strategy. We selected the technologies based on our daily challenges and needs.

We use the following technologies in this project:

Backend:
- **.NET 8.0**: A free, cross-platform, open source developer platform for building many different types of applications.
  - Git: https://github.com/dotnet/core
- **C#**: A modern object-oriented programming language developed by Microsoft.
  - Git: https://github.com/dotnet/csharplang

Testing:
- **xUnit**: A free, open source, community-focused unit testing tool for the .NET Framework.
  - Git: https://github.com/xunit/xunit

Frontend:
- **Angular 14**: A platform for building mobile and desktop web applications.
  - Git: https://github.com/angular/angular

Databases:
- **PostgreSQL**: A powerful, open source object-relational database system.
  - Git: https://github.com/postgres/postgres
  
  