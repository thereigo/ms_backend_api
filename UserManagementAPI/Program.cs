

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Error-handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var errorJson = System.Text.Json.JsonSerializer.Serialize(new { error = "Internal server error." });
        await context.Response.WriteAsync(errorJson);
        Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
    }
});

// Authentication middleware
app.Use(async (context, next) =>
{
    // Example: Expect token in Authorization header as 'Bearer <token>'
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        var errorJson = System.Text.Json.JsonSerializer.Serialize(new { error = "Unauthorized" });
        await context.Response.WriteAsync(errorJson);
        return;
    }
    var token = authHeader.Substring("Bearer ".Length).Trim();
    // For demo, accept token 'secrettoken' as valid
    if (token != "secrettoken")
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        var errorJson = System.Text.Json.JsonSerializer.Serialize(new { error = "Invalid token" });
        await context.Response.WriteAsync(errorJson);
        return;
    }
    await next();
});

// Logging middleware
app.Use(async (context, next) =>
{
    var method = context.Request.Method;
    var path = context.Request.Path;
    await next();
    var statusCode = context.Response.StatusCode;
    Console.WriteLine($"{method} {path} => {statusCode}");
});

app.UseHttpsRedirection();

var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
    new User { Id = 2, Name = "Bob", Email = "bob@example.com" },
    new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
};

// GET: Retrieve all users
app.MapGet("/users", () => users);

// GET: Retrieve a user by ID
app.MapGet("/users/{id}", (int id) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Results.Ok(user) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error retrieving user: {ex.Message}");
        return Results.Problem("Internal server error.");
    }
});

// POST: Add a new user
app.MapPost("/users", (User user) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            return Results.BadRequest("Name is required.");
        if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
            return Results.BadRequest("Valid email is required.");
        user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        return Results.Created($"/users/{user.Id}", user);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error adding user: {ex.Message}");
        return Results.Problem("Internal server error.");
    }
});

// PUT: Update an existing user
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Results.NotFound();
        if (string.IsNullOrWhiteSpace(updatedUser.Name))
            return Results.BadRequest("Name is required.");
        if (string.IsNullOrWhiteSpace(updatedUser.Email) || !updatedUser.Email.Contains("@"))
            return Results.BadRequest("Valid email is required.");
        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        return Results.Ok(user);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error updating user: {ex.Message}");
        return Results.Problem("Internal server error.");
    }
});

// DELETE: Remove a user by ID
app.MapDelete("/users/{id}", (int id) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Results.NotFound();
        users.Remove(user);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error deleting user: {ex.Message}");
        return Results.Problem("Internal server error.");
    }
});

app.Run();

record User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
