***********************************************************************************************************************************
API: POST http://localhost:5186/api/squawks

BODY
{
    "userId": "1a9a269d-a6b9-4e22-9f99-b56283e7fe21",
    "content": "This is a test squawk."
}
***********************************************************************************************************************************
 
 # ParrotInc.SquawkService

***********************************************************************************************************************************

Break Down the Project into tasks:

* User Story 1: Setup Project Structure
As a developer, I want to set up the project structure so that the code is organized and maintainable.

Tasks:
Create a solution file for the project.
Create project files for the SquawkService.
Organize folders for Domain, Application, Infrastructure, and API layers.

* User Story 2: Define the Domain
As a developer, I want to define the domain entities and value objects to represent the core business logic.

Tasks:
Create the Squawk entity class with properties.
Define value objects.
Create domain services for business rules.

* User Story 3: Implement Application Logic
As a developer, I want to implement the application logic so that the API can handle requests effectively.

Tasks:
Create command models for actions like adding a Squawk.
Create query models if needed for retrieving Squawks.
Implement command handlers.
Implement query handlers for retrieving Squawks.

* User Story 4: Setup Infrastructure
As a developer, I want to set up the infrastructure for data access and logging so that the application is robust.

Tasks:
Configure an in-memory database for testing purposes.
Implement the repository pattern for data access.
Set up a logging framework for the application.

* User Story 5: Create API Layer
As a developer, I want to define the API endpoints so that clients can interact with the service.

Tasks:
Define API endpoints.
Implement request models.
Validate incoming requests according to specifications.

* User Story 6: Implement Rate Limiting
As a developer, I want to enforce rate limiting to prevent users from posting too frequently.

Tasks:
Create a mechanism to enforce the 20-second interval between Squawks.
Store timestamps of the last Squawk for each user.

* User Story 7: Handle Content Restrictions
As a developer, I want to implement content restrictions to ensure that Squawks comply with guidelines.

Tasks:
Implement logic to reject Squawks containing “Tweet” or “Twitter.”
Check for duplicate Squawks based on UserId and Squawk text.

* User Story 8: Write Unit Tests
As a developer, I want to write unit tests to ensure that the application logic works correctly.

Tasks:
Create unit tests for command handlers.
Write tests for validation rules and restrictions.

* User Story 9: Containerize the Application
As a developer, I want to containerize the application to simplify deployment.

Tasks:
Create a Dockerfile for the SquawkService.
Write a Docker Compose file if needed for a multi-container setup.

* User Story 10: Document the Project
As a developer, I want to document the project so that other developers can understand and use it.

Tasks:
Create a README file with project overview.
Document API endpoints and usage instructions.


***********************************************************************************************************************************
Non-Functional Requirements Implementation Details
Architecture Overview

# Microservices: Each service will be independently deployable, allowing for isolated scaling and updates without affecting the entire application.
DDD (Domain-Driven Design): Focus on creating a rich domain model. Use entities and value objects to encapsulate business logic, promoting a clear understanding of the business rules.
Caching Strategy

# In-Memory Cache: Use in-memory storage (like a dictionary or set) to hold frequently accessed data, enabling quick lookups. This cache will help minimize database queries for commonly requested data.
Rate Limiting Cache: Implement an in-memory structure to store the timestamps of the last Squawk for each user. This will help enforce the 20-second posting interval, allowing for fast access without the need for persistent storage.
Design Patterns

# Repository Pattern: This pattern will be utilized to abstract the data access layer. It will provide a clear interface for data retrieval and manipulation, allowing the application to interact with data sources without knowing their details.
CQRS (Command Query Responsibility Segregation): Separate the command (write) and query (read) operations. This approach improves performance and scalability, allowing the read model to be optimized independently of the write model.
Rate Limiting Implementation

# 20-Second Interval: Use timestamps stored in memory to track the last time a user posted. Implement logic that checks this timestamp against the current time before allowing a new Squawk. If the time difference is less than 20 seconds, reject the request.
Thread Safety: Ensure that the in-memory storage is thread-safe to prevent race conditions when multiple requests attempt to read/write simultaneously. Use synchronization mechanisms to protect critical sections of the code.
Content Restrictions

# Validation: Implement checks to ensure that the Squawk text does not contain the words "Tweet" or "Twitter." If detected, reject the Squawk with an appropriate error message.
Duplicate Check: Use a hashing strategy to create a unique identifier for each Squawk based on the combination of UserId and the Squawk text. Store these identifiers in an in-memory data structure to quickly check for duplicates. If a new Squawk generates the same hash as an existing one, reject it.
Unit Testing and Quality Assurance

# Testing Strategy: Develop a comprehensive testing strategy that includes unit tests for individual components and integration tests for interactions between them. This ensures that each part functions correctly and meets the overall application requirements.
CI/CD Pipeline: Implement a Continuous Integration/Continuous Deployment pipeline to automate testing whenever code is committed. This ensures that any issues are caught early in the development process.
Documentation and Usability

# API Documentation: Use tools like Swagger to generate interactive API documentation. This will help other developers understand how to interact with the service and provide examples of requests and responses.
Error Handling: Implement consistent error handling across the API. Provide meaningful error messages and status codes to help users understand what went wrong and how to fix it.

***********************************************************************************************************************************

Out of Scope
API Gateway
Not implementing an API Gateway.
Benefits: Would manage authentication, logging, and routing in a real application.

Distributed Caching
Using in-memory caching only; no distributed cache.
Limitation: Each container has its own cache, leading to redundancy.

Complex Business Logic
No advanced workflows or metrics.
Focus: Basic Squawk management for now.

Asynchronous Processing
Not using message queues for processing.
Limitation: This would improve responsiveness and decouple tasks in a larger system.

Extensive Error Handling
Basic error handling only.
Future work: Enhance logging and response management for reliability.

***********************************************************************************************************************************

Premises
Using NoSQL for the SquawkService is better because:

Flexibility: Dynamic schema adapts to changing data structures.
Scalability: Supports horizontal scaling for high user loads.
Performance: Optimized for fast read/write operations.
Unstructured Data: Easily handles varying data formats (text, images).
Rapid Development: Allows quick iterations without strict schema constraints.
Document Storage: Aligns well with storing Squawk data as documents.
MongoDB, DynamoDB, and Cassandra are great options.

I am not considering a graph database because it will bring more complex data relationships and implementation.

I am not considering implementing a robust Kafka queue to handle high traffic of messages due to the added complexity and overhead it would introduce.

I am not considering using a relational database (e.g., PostgreSQL) because it may impose rigid schema constraints and hinder rapid development.

I am not considering using Redis as a primary data store due to its focus on caching rather than persistent storage.

I am not considering using an API Gateway because it would add an extra layer of complexity and overhead that is not necessary for this project scope.

This project does not represent a real scenario of Twitter; it is solely a design exercise to practice concepts and ideas.
