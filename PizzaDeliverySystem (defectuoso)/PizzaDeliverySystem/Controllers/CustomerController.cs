using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;

namespace PizzaDeliverySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customer/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken ct)
    {
        var customer = await _customerService.GetByIdAsync(id, ct);
        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    // POST: api/customer
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _customerService.CreateAsync(request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    // PUT: api/customer/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken ct)
    {
        if (id != request.Id)
            return BadRequest("Route id and body id must match.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _customerService.UpdateAsync(request, ct);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/customer/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _customerService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
