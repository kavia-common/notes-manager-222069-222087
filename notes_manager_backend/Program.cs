using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotesManagerBackend.DTOs;
using NotesManagerBackend.Models;
using NotesManagerBackend.Repositories;
using NotesManagerBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// App metadata for OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Notes Manager API";
    settings.Version = "v1";
    settings.Description = "A simple REST API for managing notes with CRUD operations.";
});

// CORS (permissive for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register services
builder.Services.AddSingleton<INotesRepository, InMemoryNotesRepository>();
builder.Services.AddSingleton<INotesService, NotesService>();

var app = builder.Build();

// Ensure Kestrel listens on port 3001 in all environments if not overridden
// This allows the preview system to connect reliably.
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:3001");

// Middlewares
app.UseCors("AllowAll");
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Health check endpoint
app.MapGet("/", () => Results.Ok(new { message = "Healthy" }))
    .WithName("HealthCheck")
    .WithTags("Health");

// Notes endpoints
var notesGroup = app.MapGroup("/api/notes").WithTags("Notes");

// GET /api/notes
notesGroup.MapGet("/", (INotesService service) =>
{
    var notes = service.GetAll();
    return Results.Ok(notes);
})
.WithName("GetNotes")
.WithSummary("Get all notes")
.WithDescription("Returns a list of all notes.");

// GET /api/notes/{id}
notesGroup.MapGet("/{id:guid}", (Guid id, INotesService service) =>
{
    var note = service.Get(id);
    return note is null ? Results.NotFound(new ProblemDetails
    {
        Title = "Not Found",
        Detail = $"Note with id '{id}' was not found.",
        Status = StatusCodes.Status404NotFound
    }) : Results.Ok(note);
})
.WithName("GetNoteById")
.WithSummary("Get a note by ID")
.WithDescription("Returns a single note by its unique identifier.");

// POST /api/notes
notesGroup.MapPost("/", ([FromBody] NoteCreateDto dto, INotesService service) =>
{
    // Model binding + data annotations validation
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(dto, null, null);
    if (!Validator.TryValidateObject(dto, context, validationResults, true))
    {
        var problem = new ValidationProblemDetails(
            validationResults
                .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray())
        )
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest
        };
        return Results.ValidationProblem(problem.Errors, statusCode: StatusCodes.Status400BadRequest, title: problem.Title);
    }

    var created = service.Create(dto);
    return Results.Created($"/api/notes/{created.Id}", created);
})
.WithName("CreateNote")
.WithSummary("Create a new note")
.WithDescription("Creates a new note and returns the created note with its ID.");

// PUT /api/notes/{id}
notesGroup.MapPut("/{id:guid}", (Guid id, [FromBody] NoteUpdateDto dto, INotesService service) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(dto, null, null);
    if (!Validator.TryValidateObject(dto, context, validationResults, true))
    {
        var problem = new ValidationProblemDetails(
            validationResults
                .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray())
        )
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest
        };
        return Results.ValidationProblem(problem.Errors, statusCode: StatusCodes.Status400BadRequest, title: problem.Title);
    }

    var updated = service.Update(id, dto);
    if (updated is null)
    {
        return Results.NotFound(new ProblemDetails
        {
            Title = "Not Found",
            Detail = $"Note with id '{id}' was not found.",
            Status = StatusCodes.Status404NotFound
        });
    }
    return Results.Ok(updated);
})
.WithName("UpdateNote")
.WithSummary("Update an existing note")
.WithDescription("Updates a note by ID and returns the updated note.");

// DELETE /api/notes/{id}
notesGroup.MapDelete("/{id:guid}", (Guid id, INotesService service) =>
{
    var deleted = service.Delete(id);
    return deleted
        ? Results.NoContent()
        : Results.NotFound(new ProblemDetails
        {
            Title = "Not Found",
            Detail = $"Note with id '{id}' was not found.",
            Status = StatusCodes.Status404NotFound
        });
})
.WithName("DeleteNote")
.WithSummary("Delete a note")
.WithDescription("Deletes a note by ID.");

app.Run();