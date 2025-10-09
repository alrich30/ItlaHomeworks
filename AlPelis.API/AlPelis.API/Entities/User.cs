namespace AlPelis.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Navigation property (one user can have many loans)
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
