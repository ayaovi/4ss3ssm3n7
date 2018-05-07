using System;
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
    public IActionResult Get(Guid orderId)
    {
      try
      {
        var orderlines = _repository.GetOrderLinesByOrderId(orderId);
        return Ok(orderlines);
      }
      catch (Exception e)
      {
        return NotFound($"There is no order with id {orderId}.");
      }
    }
  }
}
