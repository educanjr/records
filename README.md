# BallastLane API

## User Story
As a user, I want to use the BallastLane API to create, view, update, and delete records for my account so that I can manage my personal information. I expect that:

* Register for an account with a unique username and email address, and a password that meets certain criteria, so that I can access protected features of the application.
* Log in to my account with my registered email address and password, so that I can access protected features of the application
* When I create a new record, I must provide a title and a description. The title must be between 1 and 100 characters long, and the description must be between 1 and 500 characters long.
* When I view a record, I must provide the record ID, and the API should return the record only if it belongs to my account.
When I view all records for my account, the API should return a list of all records that belong to my account.
* When I update a record, I must provide the record ID and the new title and description. The title must be between 1 and 100 characters long, and the description must be between 1 and 500 characters long. The API should update the record only if it belongs to my account.
* When I delete a record, I must provide the record ID, and the API should delete the record only if it belongs to my account.
* Additionally, when I create a new user account, I must provide a unique  and valid email address. If any of these requirements are not met, the API should return a 400 Bad Request status code with a message indicating the error.

## Methodologies Used

* The BallastLane  API was developed using a test-driven development (TDD) approach, which ensured that all code was thoroughly tested before being implemented.
* The project also applies Clean Architecture by dividing the codebase into layers that are independent of each other. The layers are:

1. Domain: The core business logic of the application, where the entities, and business rules reside
2. Application: The layer that coordinates the use cases of the system.
3. Infrastructure:

3.1 Infrasetructure project: Implement interfaces from the Application Layer to provide functionality to access external systems.
3.2 Persistence project: Implement interfaces from the Application Layer to provide functionality to access database.

4. Presentation:

4.1. Web.App Project: Contain the core of the application, in charge of inject dependencies, run the application and orchestrate how it built.
4.2. Presentation Project: Contain controllers and endpoints, creating this separation we mitigate the direct interactions in between Presentation and Infrastructure layers.

* And lastly SOLID proinciples where also applied in several ways:

1. Single Responsibility Principle (SRP): Each class has a single responsibility and is only responsible for one thing.
2. Open-Closed Principle (OCP): The code is open for extension but closed for modification. This means that we can add new functionality without changing existing code.
3. Liskov Substitution Principle (LSP): Objects of a superclass should be able to be replaced with objects of a subclass without causing errors in the program.
4. Interface Segregation Principle (ISP): Clients should not be forced to depend on interfaces that they do not use. Interfaces are defined for specific needs.
5. Dependency Inversion Principle (DIP): High-level modules should not depend on low-level modules. Both should depend on abstractions.

## Getting Started

To run the BallastLane  API, follow these steps:

Clone the repository to your local machine.

    git clone https://github.com/educanjr/records.git

Navigate to the src folder

    cd src

Ensure that you have the latest version of Docker installed and Run the docker compose command up  

    docker-compose up --build -d

Navigate to `http://localhost:8000` in your web browser to access a simple react application taht consumes the API.
Or you could enter either

    `https://localhost:7007/swagger/index.html`
    `http://localhost:7006/swagger/index.html`

For getting access to the swagger definition of the API, and directly consume the endpoints. Take in consideration that Records endpoint need authentication, so first you wil need to consume endpoints:

    POST /api/Users/register

For creating an user. And later for getting a token:

    POST api/Authentication/login

That the response for this endpon is an string containing the value of the JWT Token (also visible on the swagger definition):

    "string"

The token is needed on the "Authorize" option of swagger, and from there all records endpoints options can be consumed.

### API Endpoints

The BallastLane.APP API provides the following endpoints:

#### Records

```
GET api/Records
Retrieves a list of all records.

GET api/Records/{id}
Retrieves a specific record by ID.

POST api/Records
Creates a new record.

PUT api/Records/{id}
Updates an existing record by ID.

DELETE api/records/{id}
Deletes an existing record by ID.
```

#### Users

```
GET api/Users/{id}
Retrieves a specific usr by ID.

POST api/Users
Creates a new user.

PUT api/Users/{id}
Updates an existing user by ID.

DELETE api/Users/login
Authenticate en user in the API and return a JWT token.
```
