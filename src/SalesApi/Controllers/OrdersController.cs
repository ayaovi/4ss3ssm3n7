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
      using (var context = new SalesContext())
      {
        //context.Clients.Load();
        //if client does not exist, do not place order.
        var clients = context.Clients;
        if (!context.Clients.Any(x => x.Id == request.ClientId))
        {
          //client does not exist.
          return NotFound();
        }

        var items = context.Items;

        if (request.OrderLineRequests.Select(x => x.Item.Id).Select(x => items.Any(y => y.Id == x)).Any(x => x == false))
        {
          //there is a supplied item that does not exist.
          return NotFound();
        }

        var order = new Order
        {
          Id = new Guid(),
          Client = clients.Single(x => x.Id == request.ClientId),
          OrderLines = new List<OrderLine>()
        };

        request.OrderLineRequests.ToList().ForEach(x =>
        {
          var orderLine = new OrderLine
          {
            Id = new Guid(),
            Item = items.Single(y => y.Id == x.Item.Id),
            Order = order,
            Quantity = x.Quantity
          };
          order.OrderLines.Add(orderLine);
          context.OrderLines.Add(orderLine);
        });

        context.Orders.Add(order);
        context.SaveChanges();
        return Ok();
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
