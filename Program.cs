using BookLibraryApi.Models;
using BookLibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

var bookApi = app.MapGroup("/books");

bookApi.MapGet("/", (BookService service) =>
{
    return service.GetAllBooks();
});

bookApi.MapGet("/{id}", (int id, BookService service) =>
{
    var book = service.GetById(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

bookApi.MapPost("/", (Book book, BookService service) =>
{
    var added = service.AddBook(book);
    return Results.Created($"/books/{added.Id}", added);
});

bookApi.MapPut("/{id}", (int id, Book book, BookService service) =>
{
    var success = service.UpdateBook(id, book);
    return success ? Results.NoContent() : Results.NotFound();
});

bookApi.MapDelete("/{id}", (int id, BookService service) =>
{
    var success = service.DeleteBook(id);
    return success ? Results.NoContent() : Results.NotFound();
});

bookApi.MapGet("/search", (string query, BookService service) =>
{
    var books = service.SearchBooks(query);
    return Results.Ok(books);
});

app.Run();