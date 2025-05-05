using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(LibraryDbContext db)
        {
            if (await db.Books.AnyAsync()) return; // already seeded

            var books = new List<Book>
            {
                new() { Title = "The Pragmatic Programmer", Author = "Andy Hunt", Genre = "Programming", YearPublished = 1999 },
                new() { Title = "Clean Code", Author = "Robert C. Martin", Genre = "Programming", YearPublished = 2008 },
                new() { Title = "1984", Author = "George Orwell", Genre = "Dystopian", YearPublished = 1949 },
                new() { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Classic", YearPublished = 1960 }
            };

            db.Books.AddRange(books);
            await db.SaveChangesAsync();
        }
    }
}
