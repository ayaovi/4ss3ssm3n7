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
    IEnumerable<OrderLine> GetOrderLinesByOrderId();
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
      throw new System.NotImplementedException();
    }

    public IEnumerable<OrderLine> GetOrderLinesByOrderId()
    {
      throw new System.NotImplementedException();
    }

    public void AddOrderLine(OrderLine orderLine)
    {
      throw new System.NotImplementedException();
    }
  }
}
