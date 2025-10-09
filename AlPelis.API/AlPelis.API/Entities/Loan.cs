namespace AlPelis.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table("loans")]
public class Loan
{
    [Column("id")]
    public int Id { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    [Column("end_date")]
    public DateTime? EndDate { get; set; }  // null if the book hasn't been returned yet

    [Column("book_id")]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    [Column("user_id")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

