using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Models
{
    public class Book
    {
        public int Id { get; set; } // unique identifier

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Author { get; set; } = string.Empty;

        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [Range(0, 3000)]
        public int YearPublished { get; set; }
    }
}
