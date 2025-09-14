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

## Debugging & Improvements (September 2025)

With the help of GitHub Copilot, the following bugs and issues were identified and fixed:

- **Validation Added:**
  - Users cannot be added or updated with empty names or invalid emails.
- **Error Handling:**
  - All endpoints now use try-catch blocks to prevent API crashes from unhandled exceptions.
  - Internal errors are logged and return a generic error response.
- **Reliability:**
  - GET, PUT, and DELETE endpoints now handle non-existent user IDs gracefully.
- **Logging:**
  - Errors are logged to the console for easier debugging.

These improvements ensure the API works reliably and meets TechHive Solutions' requirements. Copilot streamlined the debugging process by quickly identifying missing validation, error handling, and suggesting best practices for robust API design.

---

Generated and assisted by GitHub Copilot
