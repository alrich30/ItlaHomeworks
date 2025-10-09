using AlPelis.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlPelis.API.Data
{
    public class LibraryContext: DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Loan> Loans => Set<Loan>();
    }
}
