using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Persistence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

    // GET: api/<controller>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new[] { "value1", "value2" };
    }

    // GET api/<controller>/5
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

    // POST api/<controller>
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
