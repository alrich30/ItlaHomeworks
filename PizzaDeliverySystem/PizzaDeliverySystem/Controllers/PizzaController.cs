using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;

namespace PizzaDeliverySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService _pizzaService;

    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    // GET: api/pizza
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaDto>>> GetAll(CancellationToken ct)
    {
        var pizzas = await _pizzaService.GetAllAsync(ct);
        return Ok(pizzas);
    }


    // GET: api/pizza/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PizzaDto>> GetById(Guid id, CancellationToken ct)
    {
        var pizza = await _pizzaService.GetByIdAsync(id, ct);
        if (pizza is null)
            return NotFound();

        return Ok(pizza);
    }

    // POST: api/pizza
    [HttpPost]
    public async Task<ActionResult<PizzaDto>> Create([FromBody] CreatePizzaRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _pizzaService.CreateAsync(request, ct);

        // Devuelve 201 con la URL del recurso creado
        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    // PUT: api/pizza/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PizzaDto>> Update(Guid id, [FromBody] UpdatePizzaRequest request, CancellationToken ct)
    {
        if (id != request.Id)
            return BadRequest("Route id and body id must match.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _pizzaService.UpdateAsync(request, ct);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/pizza/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _pizzaService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
