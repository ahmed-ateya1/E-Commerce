Here’s the updated `README.md` file with a section added for **Docker** usage. This section explains how to build and run the application using Docker, making it easier for developers to set up the project in a containerized environment.

---

# E-Commerce.API

## Overview
E-Commerce.API is a full-featured backend application designed for an online shopping platform. It provides APIs to manage user accounts, products, orders, payments, and more. The application leverages a clean architecture to ensure scalability, maintainability, and testability.

---

## Technologies Used

### Backend Framework
- **ASP.NET Core 9.0**: Used to build RESTful APIs and provide middleware for handling HTTP requests.

### Database
- **Entity Framework Core**: An Object-Relational Mapper (ORM) for data access and management.
- **SQL Server**: The relational database used for storing data.

### Payment Gateway
- **Stripe**: Integrated for payment processing, including payment intents and refunds.

### Authentication and Authorization
- **JWT (JSON Web Tokens)**: Used for secure user authentication.

### Caching
- **MemoryCache**: For in-memory caching of frequently accessed data.

### Logging
- **Serilog**: For structured and file-based logging.

### Others
- **AutoMapper**: For mapping between DTOs and entities.
- **SignalR**: For real-time application features such as notifications.
- **Swagger**: For API documentation.
- **Fluent Validation**: Provides fluent validation for defining model validation.

---

## Project Architecture

The project follows a clean architecture approach, separating concerns into multiple layers:

### 1. **Controllers**
   - Handle HTTP requests and responses.
   - Validate user inputs and interact with services.

### 2. **Core Layer**
   - Contains business logic, services, and contracts.
   - Includes caching, domain models, DTOs, and helpers.

### 3. **Services**
   - Business logic for various features such as payments, orders, and product management.

### 4. **Repository Pattern**
   - Provides data access logic via a unit of work and repositories.

### 5. **CQRS (Command Query Responsibility Segregation)**
   - Separates commands (write operations) and queries (read operations) to improve scalability and maintainability.
   - Commands handle updates, inserts, and deletions, while queries handle data retrieval.

### 6. **Generic Repository**
   - Implements a generic approach for data access, reducing code duplication.
   - Provides CRUD operations for all entities, making the repository reusable and extensible.

### 7. **Unit of Work**
   - Manages transactions across multiple repositories, ensuring consistency.
   - Simplifies data operations by grouping multiple changes into a single transaction.

---

## Features and Entities

### 1. **Address**
- Represents user delivery addresses.
- Fields include `Street`, `City`, `State`, `PostalCode`, and `Country`.

### 2. **Brand**
- Manages product brands.
- Fields include `Name` and `Description`.

### 3. **CartItems**
- Represents items in the shopping cart.
- Includes product references, quantities, and prices.

### 4. **ShoppingCart**
- Manages user shopping carts.
- Includes associations with `CartItems` and user information.

### 5. **Category**
- Represents product categories.
- Fields include `Name` and `ParentCategory` for hierarchical relationships.

### 6. **DeliveryMethod**
- Stores delivery methods and their associated costs.
- Fields include `Name`, `EstimatedTime`, and `Price`.

### 7. **Order**
- Represents user orders.
- Fields include `OrderDate`, `Status`, `TotalAmount`, and a list of `OrderItems`.

### 8. **OrderItems**
- Represents individual items in an order.
- Includes references to `Product`, `Quantity`, and `Price`.

### 9. **Notification**
- Manages notifications for users.
- Fields include `Message`, `Type`, and `IsRead`.

### 10. **Product**
- Represents products in the store.
- Fields include `Name`, `Description`, `Price`, `StockQuantity`, and `Category`.

### 11. **ProductImages**
- Stores images associated with products.
- Fields include `ImageUrl` and `ProductId`.

### 12. **TechnicalSpecification**
- Stores detailed specifications of products.
- Fields include `Key` and `Value` pairs.

### 13. **Vote**
- Manages user ratings for products.
- Fields include `Rating`, `Comment`, and `UserId`.

### 14. **Wishlist**
- Represents a user's wishlist.
- Fields include a list of `Product` references and user information.

### 15. **Review**
- Stores reviews for products.
- Fields include `Content`, `Rating`, `ProductId`, and `UserId`.

### 16. **Deal**
- Represents a promotional deal associated with a product in an e-commerce system.

---

## Key Functionalities

### Authentication
- JWT-based authentication for secure access to APIs.

### Product Management
- Endpoints to create, update, delete, and view products.
- Category and brand filtering.

### Cart and Wishlist
- Add, update, and remove items from the shopping cart and wishlist.

### Orders
- Create and manage user orders.
- Includes payment processing and order status tracking.

### Payments
- Stripe integration for handling payments.
- Webhook to handle events like payment success, failure, and refunds.

### Notifications
- Real-time notifications for order updates and other events.

---

## Folder Structure

### 1. **Controllers**
- Handles API endpoints for each feature. Example:
  - `AccountController`: Manages user accounts.
  - `PaymentController`: Handles Stripe payments.
  - `ProductController`: Manages products and categories.

### 2. **Core**
- Contains business logic, helper classes, and domain entities.
- Subfolders:
  - `Caching`: Services for in-memory caching.
  - `Commands`: CQRS commands for specific actions.
  - `Domain`: Domain entities like `Order`, `Product`, and `Category`.
  - `Services`: Business logic for various features.

### 3. **Infrastructure**
- Includes repositories, database context, and migrations.

---

## Docker Support

The application is containerized using Docker, making it easy to set up and run in any environment.

### Prerequisites
- Install [Docker](https://docs.docker.com/get-docker/) on your machine.

### Steps to Run the Application with Docker

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ahmed-ateya1/E-Commerce.git
   cd E-Commerce
   ```

2. **Build the Docker Image**
   ```bash
   docker build -t e-commerce-api .
   ```

3. **Run the Docker Container**
   ```bash
   docker run -p 5000:80 e-commerce-api
   ```

4. **Access the Application**
   - Open your browser and navigate to `http://localhost:5000`.
   - Use Swagger UI at `http://localhost:5000/swagger` to explore the API endpoints.

### Docker Compose (Optional)
If you want to run the application along with a SQL Server database, use Docker Compose:

1. **Create a `docker-compose.yml` File**
   ```yaml
   version: '3.8'
   services:
     db:
       image: mcr.microsoft.com/mssql/server:2022-latest
       environment:
         SA_PASSWORD: "YourStrong!Passw0rd"
         ACCEPT_EULA: "Y"
       ports:
         - "1433:1433"
     app:
       build: .
       ports:
         - "5000:80"
       depends_on:
         - db
   ```

2. **Run Docker Compose**
   ```bash
   docker-compose up
   ```

---

## API Documentation
- API documentation is available via Swagger at `/swagger`.

---

## License
This project is licensed under the MIT License.

---

This updated `README.md` now includes a **Docker Support** section, providing clear instructions for building and running the application using Docker. It also includes an optional Docker Compose setup for running the application with a SQL Server database.
