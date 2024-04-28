# Transportation Management System API

Welcome to the Transportation Management System API! This API facilitates the management of transportation services through three main systems:

1. Admin System
2. Bus Stop System
3. Online User System

## Admin System

The Admin System allows for the creation of new managers and provides access to reports regarding the system's performance and profit.

### Endpoints:

- `/api/Admin/create-manager`: Create a new Bus stop Manager
- `/api/Admin/enroll-bus-stop-to-bus-stop`: Add a connection between 2 bus stops.
- `/api/Admin/get-all-history-journeys`: Get all journeys from DB.
- `/api/Admin/get-all-buses`: Get all Buses from DB.

## Bus Stop System

The Bus Stop System manages each individual bus stop, providing access to create buses, journeys, and ticketing services.

### Endpoints:

- `/api/Manager/add-bus`: Create a new Bus to the system and bus stop.
- `/api/Manager/add-journey`: Create a new journey to the upcoming journeys.
- `/api/Manager/cut-ticket`: Generate a new ticket.

## Online User System

The Online User System caters to users who wish to book tickets and obtain information about available journeys between bus stops.

### Endpoints:

- `/api/User/book-ticket`: Book a ticket for a specific journey.
- `/api/UpcomingJourney/get-all-upcoming-journeys`: Get information about available journeys between bus stops.


### Note:
- These end points , not all the endpoints in the project , go to documention to explore all the endpoints ☺️.

## Authentication

Authentication is required for accessing certain endpoints. Each system has its own authentication mechanism to ensure secure access.

## Rate Limiting

To prevent abuse and ensure fair usage, rate limiting is applied to certain endpoints. Please check the Documentation

## Documentation

For detailed information about each endpoint, authentication methods, and more, [Documentation](http://transportationsystem.somee.com/swagger/index.html)

## Contributors

We'd like to acknowledge the following individuals for their contributions to this project:

- [Amr Shalaby](https://github.com/amr1234567)
- [Mohamed El-Sayed](https://github.com/mhmdelsyd)
