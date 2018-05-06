using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApi.Contexts;
using SalesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesApi.Controllers
{
  [Route("api/[controller]")]
  public class OrderLinesController : Controller
  {
    // GET: api/<controller>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public IEnumerable<OrderLine> Get(Guid id)
    {
      using (var context = new SalesContext())
      {
        context.Materials.Load();
        context.Items.Load();
        context.OrderLines.Load();
        context.Clients.Load();
        var orderLines = context.Orders.Single(x => x.Id == id).OrderLines.ToList();
        return orderLines;
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
