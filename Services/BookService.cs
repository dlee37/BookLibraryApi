using BookLibraryApi.Models;

namespace BookLibraryApi.Services
{
    public class BookService
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;
        public List<Book> GetAllBooks() => _books;

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public Book AddBook(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            return book;
        }

        public bool UpdateBook(int id, Book updatedBook)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                return false;
            }
            existing.Title = updatedBook.Title;
            existing.Author = updatedBook.Author;
            existing.Genre = updatedBook.Genre;
            existing.YearPublished = updatedBook.YearPublished;

            return true;
        }

        public bool DeleteBook(int id)
        {
            var book = GetById(id);
            if (book == null)
            {
                return false;
            }
            _books.Remove(book);
            return true;
        }

        public List<Book> SearchBooks(String query)
        {
            return _books.Where(b =>
                b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
