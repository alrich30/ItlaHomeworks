using AlPelis.API.Data;
using AlPelis.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlPelis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly LibraryContext _ctx;
    public LoansController(LibraryContext ctx) => _ctx = ctx;

    // List all loans (includes book and user)
    [HttpGet]
    public async Task<IEnumerable<object>> Get() =>
        await _ctx.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Select(l => new {
                l.Id,
                l.StartDate,
                l.EndDate,
                Book = new { l.Book.Id, l.Book.Title },
                User = new { l.User.Id, l.User.Name }
            })
            .ToListAsync();

    // Create a loan (marks book as unavailable)
    [HttpPost]
    public async Task<ActionResult<Loan>> Post(Loan l)
    {
        var book = await _ctx.Books.FindAsync(l.BookId);
        var user = await _ctx.Users.FindAsync(l.UserId);
        if (book is null || user is null)
            return BadRequest("Book or User does not exist.");
        if (!book.Available)
            return BadRequest("The book is not available.");

        book.Available = false;
        _ctx.Loans.Add(l);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = l.Id }, l);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Loan>> GetById(int id)
    {
        var loan = await _ctx.Loans.FindAsync(id);
        if (loan == null)
        {
            return NotFound();
        }
        return loan;
    }
    
    // Return book: sets EndDate and marks it as available again
    [HttpPut("{id:int}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _ctx.Loans.Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == id);
        if (loan is null) return NotFound();
        if (loan.EndDate is not null)
            return BadRequest("The loan has already been closed.");

        loan.EndDate = DateTime.UtcNow;
        loan.Book.Available = true;
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var loan = await _ctx.Loans.FindAsync(id);
        if (loan is null) return NotFound();
        _ctx.Loans.Remove(loan);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
