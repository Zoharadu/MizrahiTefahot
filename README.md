# 🧮 MizrahiTefahot .NET 8 – Calculation API

> A clean and well-structured .NET 8 REST API for basic arithmetic operations, designed for scalability.

## ✅ Highlights

- 🔹 RESTful API using  .NET Core API 8
- 🔹 Input: `number1`, `number2` in the body + `operator` passed via HTTP header.
- 🔹 Operation handling via `Dictionary<string, Func<double, double, double>>` for clean and efficient logic.
- 🔹 JWT Authentication with Bearer token and expiration.
- 🔹 Auto-generated Swagger UI from YAML via SwaggerHub.
- 🔹 Full Unit Test coverage.
- 🔹 Supports asynchronous structure using `Task` for future extensibility.
- 🔹 Clean, modular, and well-documented code.

## 🛠️ Technologies Used

- .NET 8 (ASP.NET Core)
- C#
- Swagger / OpenAPI / YAML
- JWT Authentication (Bearer tokens)
- Unit Testing with xUnit
- Docker & Docker Compose
- Postman (for testing)

## 🐳 Running with Docker

1. Make sure Docker is installed and running.
2. In the project root, you’ll find:
   - `Dockerfile`
   - `docker-compose.yml`
3. Build and run the container:
docker-compose up --build
Once up, open your browser and navigate to:
http://localhost:5000/swagger/index.html

