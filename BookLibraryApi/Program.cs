using BookLibraryApi.Data;
using BookLibraryApi.Middleware;
using BookLibraryApi.Models;
using BookLibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BookService>();
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var bookApi = app.MapGroup("/books");

bookApi.MapGet("/", async (LibraryDbContext db, int? page, int? pageSize, BookService service) =>
{
    var books = await service.GetAllBooks(page ?? 1, pageSize ?? 10);
    return Results.Ok(books);
});

bookApi.MapGet("/{id}", (int id, BookService service) =>
{
    var book = service.GetById(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

bookApi.MapPost("/", (Book book, HttpContext context, BookService service) =>
{
    IDictionary<string, string[]> errors = new Dictionary<string, string[]>();
    if (!context.Request.HasJsonContentType() || !MiniValidator.TryValidate(book, out errors))
    {
        return Results.ValidationProblem(errors);
    }
    var added = service.AddBook(book);
    return Results.Created($"/books/{added.Id}", added);
});

bookApi.MapPut("/{id}", async (int id, Book book, BookService service, HttpContext context) =>
{
    IDictionary<string, string[]> errors = new Dictionary<string, string[]>();
    if (!context.Request.HasJsonContentType() || !MiniValidator.TryValidate(book, out errors)) 
    {
        return Results.ValidationProblem(errors);
    }
    var success = await service.UpdateBook(id, book);
    return success ? Results.NoContent() : Results.NotFound();
});

bookApi.MapDelete("/{id}", async (int id, BookService service) =>
{
    var success = await service.DeleteBook(id);
    return success ? Results.NoContent() : Results.NotFound();
});

bookApi.MapGet("/search", (string query, BookService service) =>
{
    var books = service.SearchBooks(query);
    return Results.Ok(books);
});

bookApi.MapGet("/crash", () =>
{
    throw new Exception("Boom");
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await DbInitializer.SeedDataAsync(db);
}


app.Run();