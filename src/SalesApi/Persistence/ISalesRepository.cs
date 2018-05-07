using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesApi.Contexts;
using SalesApi.Models;

namespace SalesApi.Persistence
{
  public interface ISalesRepository
  {
    IEnumerable<Order> GetOrders();
    void AddOrder(Order order);
    IEnumerable<OrderLine> GetOrderLines();
    IEnumerable<OrderLine> GetOrderLinesByOrderId(Guid orderId);
    void AddOrderLine(OrderLine orderLine);
  }

  public class SalesRepository : ISalesRepository
  {
    public IEnumerable<Order> GetOrders()
    {
      using (var context = new SalesContext())
      {
        context.Materials.Load();
        context.Items.Load();
        return context.Orders
                      .Include(x => x.Client)
                      .Include(x => x.OrderLines)
                      .ToList();
      }
    }

    public void AddOrder(Order order)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<OrderLine> GetOrderLines()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<OrderLine> GetOrderLinesByOrderId(Guid orderId)
    {
      using (var context = new SalesContext())
      {
        context.Materials.Load();
        context.Items.Load();
        context.OrderLines.Load();
        context.Clients.Load();
        var orderLines = context.Orders.Single(x => x.Id == orderId).OrderLines.ToList();
        return orderLines;
      }
    }

    public void AddOrderLine(OrderLine orderLine)
    {
      throw new System.NotImplementedException();
    }
  }
}
