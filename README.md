# BallastLane APP

## User Story

### User registration

* Register for an account using a unique email address and password to access protected application features.

## Account login

* Authenticate and log in to the account using the registered email address and password for secure access to application features.

## Create a new record

* Create a new record by providing a title and description.

## View a specific record

* Retrieve a specific record by supplying the record ID, ensuring that it should be only reached by Authenticated users.

## View all records

* Obtain a comprehensive list of all records, ensuring that it should be only reached by Authenticated users.

## Update a record

* Modify a record by providing the record ID along with updated title and description fields. The API will process the update only if the record belongs to the user's account, ensuring data integrity.

## Delete a record

* Remove a record by specifying the record ID, allowing deletion only if it pertains to the user's account for proper data management.

## User account creation requirements

* During account creation, provide a unique and valid email address meeting the specified criteria.

## API responses

* If any of the requirements are not met, the API will return a proper response message.

** 400 Bad Request: If the data don't complain with the especifocations.

** 403 Forbiden: If the user is not the owner to the resource to access.

** 401 Unauthorized: If the request don't belong to an auhtorized user.

** 200 - 204: To in form de correct behavior of the application.

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
