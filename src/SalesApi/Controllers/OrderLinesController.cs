using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Persistence;

namespace SalesApi.Controllers
{
  [Route("api/[controller]")]
  public class OrderLinesController : Controller
  {
    private readonly ISalesRepository _repository;

    public OrderLinesController(ISalesRepository repository)
    {
      _repository = repository;
    }
    
    /// <summary>
    ///   Retrieves all orderlines for a given order id.
    /// </summary>
    /// <param name="orderId">Identifies the order in question.</param>
    /// <returns>A collection of orderlines</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid orderId)
    {
      try
      {
        var orderlines = await _repository.GetOrderLinesByOrderIdAsync(orderId);
        return Ok(orderlines);
      }
      catch (Exception)
      {
        return NotFound($"There is no order with id {orderId}.");
      }
    }
  }
}
