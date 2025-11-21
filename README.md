# YARP API Gateway – Microservices Demo

A lightweight API Gateway built with **YARP (Yet Another Reverse Proxy)** that forwards requests to two backend microservices.

- **Gateway** → http://localhost:5001  
- **Customer Service** → http://localhost:5002  
- **Order Service** → http://localhost:5003  

## Getting Started

### Prerequisites
- .NET SDK  
- Any IDE (Visual Studio, VS Code, Rider)

### Restore & Run
```bash
dotnet restore

dotnet run --project YarpGateway
dotnet run --project CustomerService
dotnet run --project OrderService
