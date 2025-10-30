# User Management Microservice

This project is a C# microservice for user management, designed to adhere to industry standards for security and communication. It utilizes gRPC for inter-service communication, Serilog for logging, and Oracle Database with Entity Framework Core for data access. The microservice includes features for user password authentication, user information queries, and is structured to support mobile banking applications.

## Project Structure

```
user-management-microservice
├── src
│   ├── UserManagement.Service        # The main service for user management
│   │   ├── Controllers                # Contains API controllers
│   │   ├── Protos                     # gRPC service definitions
│   │   ├── Services                   # Business logic services
│   │   ├── Data                       # Database context and migrations
│   │   ├── Models                     # Entity models
│   │   ├── DTOs                       # Data Transfer Objects
│   │   ├── Repositories               # Data access interfaces and implementations
│   │   ├── Security                   # Security-related classes
│   │   ├── Logging                    # Logging configuration
│   │   ├── Config                     # Configuration files
│   │   ├── Program.cs                 # Entry point of the service
│   │   └── UserManagement.Service.csproj # Project file
│   ├── UserManagement.Client          # Client application for interacting with the service
│   └── UserManagement.Tests           # Unit tests for the service
├── docker                             # Docker configuration files
├── .gitignore                         # Git ignore file
├── README.md                          # Project documentation
├── global.json                        # SDK version specification
└── Directory.Build.props              # Common properties for projects
```

## Features

- **User Registration**: Allows new users to register with a username, password, and email.
- **User Login**: Authenticates users and provides JWT tokens for secure communication.
- **User Information Retrieval**: Enables fetching user details based on user ID or username.
- **Secure Password Handling**: Implements password hashing and verification using industry-standard algorithms.
- **Logging**: Utilizes Serilog for structured logging to monitor application behavior and troubleshoot issues.

## Technologies Used

- **C#**: The primary programming language for the microservice.
- **gRPC**: For efficient inter-service communication.
- **Entity Framework Core**: For data access and ORM capabilities with Oracle Database.
- **Serilog**: For logging and monitoring.
- **Oracle Database**: For persistent data storage.

## Getting Started

1. **Clone the Repository**:
   ```
   git clone <repository-url>
   cd user-management-microservice
   ```

2. **Set Up the Database**:
   - Ensure Oracle Database is installed and running.
   - Update the connection string in `src/UserManagement.Service/Config/appsettings.json`.

3. **Build the Project**:
   ```
   dotnet build
   ```

4. **Run the Service**:
   ```
   dotnet run --project src/UserManagement.Service/UserManagement.Service.csproj
   ```

5. **Access the Client**:
   - The client application can be run similarly to interact with the user management service.

## Testing

- Unit tests are provided in the `UserManagement.Tests` project. Run the tests using:
  ```
  dotnet test src/UserManagement.Tests/UserManagement.Tests.csproj
  ```

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.