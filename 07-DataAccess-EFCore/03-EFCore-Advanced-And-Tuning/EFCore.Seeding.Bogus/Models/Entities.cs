namespace EFCore.Seeding.Bogus.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = [];
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PublishedDate { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public List<Review> Reviews { get; set; } = [];
}

public class Review
{
    public int Id { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; } // 1 - 5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }
}
