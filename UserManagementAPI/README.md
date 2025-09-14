# UserManagementAPI

This is an ASP.NET Core Web API for managing users, created with the help of GitHub Copilot.

## Features

- **CRUD Endpoints for Users**
  - `GET /users`: Retrieve all users
  - `GET /users/{id}`: Retrieve a user by ID
  - `POST /users`: Add a new user
  - `PUT /users/{id}`: Update an existing user's details
  - `DELETE /users/{id}`: Remove a user by ID
- **In-memory user store** with sample users (Alice, Bob, Charlie)
- **OpenAPI (Swagger) UI** for interactive API documentation and testing

## How to Run

1. Start the API:
   ```bash
   dotnet run --project UserManagementAPI
   ```
2. Access Swagger UI in your browser:
   - [http://localhost:5036/swagger](http://localhost:5036/swagger)
3. Use tools like curl, Postman, or Swagger UI to interact with the endpoints.

## Notes

- The API uses HTTPS redirection. If you see a warning about HTTPS port, you can access the API via HTTP for local development.
- Data is stored in-memory and resets on each run. For production, add persistent storage.

## Next Steps

- Add database integration for persistent user management
- Implement authentication and input validation

---

Generated and assisted by GitHub Copilot
