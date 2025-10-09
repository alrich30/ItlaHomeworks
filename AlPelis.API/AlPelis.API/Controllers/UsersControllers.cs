using AlPelis.API.Data;
using AlPelis.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlPelis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly LibraryContext _ctx;
    public UsersController(LibraryContext ctx) => _ctx = ctx;

    [HttpGet]
    public async Task<IEnumerable<User>> Get() =>
        await _ctx.Users.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var u = await _ctx.Users.FindAsync(id);
        if (u == null)
            return NotFound();
        return u;

    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User u)
    {
        _ctx.Users.Add(u);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = u.Id }, u);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, User u)
    {
        if (id != u.Id) return BadRequest();
        _ctx.Entry(u).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var u = await _ctx.Users.FindAsync(id);
        if (u is null) return NotFound();
        _ctx.Users.Remove(u);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
