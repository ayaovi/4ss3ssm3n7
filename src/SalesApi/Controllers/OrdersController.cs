using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Contexts;
using SalesApi.Models;
using SalesApi.Persistence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
    // GET: api/<controller>
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var orders = _repository.GetOrders();
        return Ok(orders);
      }
      catch (Exception e)
      {
        return NotFound("Unable to retrieve orders.");
      }
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<controller>
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
