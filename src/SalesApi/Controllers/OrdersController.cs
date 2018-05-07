using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Models;
using SalesApi.Persistence;

namespace SalesApi.Controllers
{
  [Route("api/[controller]")]
  public class OrdersController : Controller
  {
    private readonly ISalesRepository _repository;

    public OrdersController(ISalesRepository repository)
    {
      _repository = repository;
    }

    /// <summary>
    ///   Retrieves all orders in the system.
    /// </summary>
    /// <returns>A collection of orders.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var orders = await _repository.GetOrdersAsync();
        return Ok(orders);
      }
      catch (Exception)
      {
        return NotFound("Unable to retrieve orders.");
      }
    }

    /// <summary>
    ///   Create a new order based on the order resquest.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]OrderRequest request)
    {
      try
      {
        await _repository.AddOrderAsync(request);
        return Ok();
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    /// <summary>
    ///   Updates a specified client's order.
    /// </summary>
    /// <param name="request">Contains all information relating to the order to update.</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]OrderRequest request)
    {
      try
      {
        await _repository.UpdateOrderAsync(request);
        return Ok();
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    /// <summary>
    ///   Deletes a specified order.
    /// </summary>
    /// <param name="orderId">Identifies the order to delete.</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] Guid orderId)
    {
      try
      {
        await _repository.DeleteOrderAsync(orderId);
        return Ok();
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
