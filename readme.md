# Transportation Management System API

Welcome to the Transportation Management System API! This API facilitates the management of transportation services through three main systems:

1. Manager System
2. Bus Stop System
3. Online User System

## Manager System

The Manager System allows for the creation of new managers and provides access to reports regarding the system's performance and profit.

### Endpoints:

- `/manager/create`: Create a new manager account.
- `/manager/reports/system`: Get reports about the system's performance.
- `/manager/reports/profit`: Get reports about the system's profit.

## Bus Stop System

The Bus Stop System manages each individual bus stop, providing access to create buses, journeys, and ticketing services.

### Endpoints:

- `/busstop/create/bus`: Create a new bus at a specific bus stop.
- `/busstop/create/journey`: Create a journey from one bus stop to another.
- `/busstop/ticketing/cut`: Cut tickets for passengers at a specific bus stop.

## Online User System

The Online User System caters to users who wish to book tickets and obtain information about available journeys between bus stops.

### Endpoints:

- `/user/book/ticket`: Book a ticket for a specific journey.
- `/user/journey/info`: Get information about available journeys between bus stops.

## Authentication

Authentication is required for accessing certain endpoints. Each system has its own authentication mechanism to ensure secure access.

## Rate Limiting

To prevent abuse and ensure fair usage, rate limiting is applied to certain endpoints. Please refer to the documentation for specific rate limits.

## Documentation

For detailed information about each endpoint, authentication methods, rate limits, and more, please refer to the API documentation.

## Contact

If you have any questions, concerns, or feedback, please contact our support team at support@transportationapi.com.

Thank you for using our Transportation Management System API! 🚌📊