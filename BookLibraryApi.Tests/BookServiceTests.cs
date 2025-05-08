using System.Runtime.InteropServices;
using BookLibraryApi.Data;
using BookLibraryApi.Models;
using BookLibraryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Tests;

public class BookServiceTests
{
    private static LibraryDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LibraryDbContext(options);
    }

    [Fact]
    public async Task AddBookAsync_ShouldAdd_ReturnBook()
    {
        var db = CreateDbContext();
        var service = new BookService(db);
        var newBook = new Book
        {
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction",
            YearPublished = 2023
        };

        var result = await service.AddBook(newBook);
        var books = await db.Books.ToListAsync();

        Assert.NotNull(result);
        Assert.Single(books);
        Assert.Equal("Test Book", books[0].Title);
    }
}
