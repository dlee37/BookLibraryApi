using System.Threading.Tasks;
using BookLibraryApi.Data;
using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Services
{
    public class BookService
    {
        private readonly LibraryDbContext _db;

        public async Task<List<Book>> GetAllBooks(int page, int pageSize)
        {
            const int maxPageSize = 50;
            int safePage = page < 1 ? 1 : page;
            int safeSize = Math.Min(pageSize < 1 ? 10 : pageSize, maxPageSize);

            return await _db.Books
                .Skip((safePage - 1) * safeSize)
                .Take(safeSize)
                .ToListAsync();
        }

        public BookService(LibraryDbContext db)
        {
            _db = db;
        }

        public async Task<Book?> GetById(int id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<Book> AddBook(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<bool> UpdateBook(int id, Book updatedBook)
        {
            var existing = await GetById(id);
            if (existing == null)
            {
                return false;
            }
            existing.Title = updatedBook.Title;
            existing.Author = updatedBook.Author;
            existing.Genre = updatedBook.Genre;
            existing.YearPublished = updatedBook.YearPublished;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await GetById(id);
            if (book == null)
            {
                return false;
            }
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Book>> SearchBooks(String query)
        {
            string lowercaseQuery = query.ToLower();
            return await _db.Books
                .Where(b => b.Title.ToLower().Contains(lowercaseQuery) ||
                            b.Author.ToLower().Contains(lowercaseQuery) ||
                            b.Genre.ToLower().Contains(lowercaseQuery))
                .ToListAsync();
        }
    }
}
