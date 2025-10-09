namespace AlPelis.API.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public bool Available { get; set; } = true;

    // Navigation property (one book can have many loans)
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}

