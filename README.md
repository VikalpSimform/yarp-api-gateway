# YARP API Gateway – Microservices Demo

A lightweight API Gateway built with **YARP (Yet Another Reverse Proxy)** that forwards and manages traffic between two backend microservices.

### Project URLs
- **Gateway** → http://localhost:5001  
- **Customer Service** → http://localhost:5002  
- **Order Service** → http://localhost:5003  

---

## Features

### 🔀 Dynamic Routing
Route requests based on **paths**, **hosts**, or **custom rules** to cleanly direct traffic to Customer and Order microservices.

### ❤️‍🩹 Active & Passive Health Checks
Automatically detect unhealthy service instances and **reroute traffic** without manual intervention.

### 🔧 Request & Response Transformations
Modify headers, rewrite URLs, or adjust payloads **at the gateway layer**, without touching backend microservices.

### 🔐 Centralized Authentication & Authorization
Apply **JWT**, **OAuth2**, or **role-based access** policies at the gateway to keep microservices simpler and secure.

### 🚦 Rate Limiting & Throttling
Avoid overload by defining global or per-route request limits to protect backend services.

### ⚖️ Clusters & Load Balancing
Group backend services and distribute traffic using strategies like **round-robin**, **random**, or **least-requests**.

---

## Getting Started

### Prerequisites
- .NET SDK  
- Visual Studio / VS Code / Rider  

---

## Run the Services

### 1. Restore dependencies
```bash
dotnet restore

### 2. Run projects
```bash
dotnet run --project YarpGateway
dotnet run --project CustomerService
dotnet run --project OrderService

