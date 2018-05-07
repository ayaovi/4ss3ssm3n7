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

    void AddOrder(OrderRequest order);

    IEnumerable<OrderLine> GetOrderLines();

    IEnumerable<OrderLine> GetOrderLinesByOrderId(Guid orderId);

    void AddOrderLine(OrderLine orderLine);

    void UpdateOrder(OrderRequest request);
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

    public void AddOrder(OrderRequest request)
    {
      using (var context = new SalesContext())
      {
        var clients = context.Clients;
        if (!context.Clients.Any(x => x.Id == request.ClientId))
        {
          //client does not exist.
          throw new Exception($"Client with {request.ClientId} does not exist.");
        }

        var items = context.Items;

        if (request.OrderLineRequests.Select(x => x.Item.Id).Select(x => items.Any(y => y.Id == x)).Any(x => x == false))
        {
          // there is a supplied item that does not exist.
          throw new Exception("Not all specified items exist.");
        }

        var order = new Order
        {
          Id = Guid.NewGuid(),
          Client = clients.Single(x => x.Id == request.ClientId),
          OrderLines = new List<OrderLine>()
        };

        request.OrderLineRequests.ToList().ForEach(x =>
        {
          var orderLine = new OrderLine
          {
            Id = Guid.NewGuid(),
            Item = items.Single(y => y.Id == x.Item.Id),
            Order = order,
            Quantity = x.Quantity
          };
          order.OrderLines.Add(orderLine);
          context.OrderLines.Add(orderLine);
        });

        context.Orders.Add(order);
        context.SaveChanges();
      }
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
      throw new NotImplementedException();
    }

    public void UpdateOrder(OrderRequest request)
    {
      using (var context = new SalesContext())
      {
        context.Materials.AsNoTracking().Load();
        context.Items.AsNoTracking().Load();
        context.OrderLines.Load();
        context.Clients.AsNoTracking().Load();

        var items = context.Items;
        var order = context.Orders.Single(x => x.Id == request.OrderId);
        var orderLines = order.OrderLines;
        request.OrderLineRequests.ToList().ForEach(x =>
        {
          var orderLine = orderLines.SingleOrDefault(y => y.Id == x.OrderLineId);
          if (orderLine != default(OrderLine))
          {
            orderLine.Quantity = x.Quantity;
            orderLine.Item = items.Single(k => k.Id == x.Item.Id);
          }
          else
          {
            orderLines.Add(new OrderLine
            {
              Id = x.OrderLineId,
              Quantity = x.Quantity,
              Item = items.Single(k => k.Id == x.Item.Id) 
            });
          }
        });

        context.Orders.Update(order);
        context.SaveChanges();
      }
    }
  }
}
