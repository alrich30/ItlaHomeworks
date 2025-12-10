using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaDeliverySystem.Application.Contract;
using PizzaDeliverySystem.Application.Dtos;
using PizzaDeliverySystem.Application.Dtos.Order;

namespace PizzaDeliverySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/order/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderService.GetByIdAsync(id, ct);
        if (order is null)
            return NotFound();

        return Ok(order);
    }

    // POST: api/order
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _orderService.CreateAsync(request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    // PUT: api/order/{id}/status
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<OrderDto>> ChangeStatus(Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Aseguramos que el Id del body coincida con la ruta
        request.Id = id;

        var updated = await _orderService.ChangeStatusAsync(request, ct);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/order/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _orderService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
