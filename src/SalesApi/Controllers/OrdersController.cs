using System;
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
    public IActionResult Get()
    {
      try
      {
        var orders = _repository.GetOrders();
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
    public IActionResult Post([FromBody]OrderRequest request)
    {
      try
      {
        _repository.AddOrder(request);
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
    public IActionResult Put([FromBody]OrderRequest request)
    {
      try
      {
        _repository.UpdateOrder(request);
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
    public IActionResult Delete([FromBody] Guid orderId)
    {
      try
      {
        _repository.DeleteOrder(orderId);
        return Ok();
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
