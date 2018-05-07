using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesApi.Contexts;
using SalesApi.Models;

namespace SalesApi.Persistence
{
  public class SalesRepository : ISalesRepository
  {
    public static void CreateDatabase()
    {
      using (var context = new SalesContext())
      {
        context.Database.EnsureCreated();
        context.SaveChanges();
      }
    }

    /// <summary>
    ///   Retrieves all orders in the system.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    ///   Creates an order from the request parameters and adds it to the database.
    /// </summary>
    /// <param name="request"></param>
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

    /// <summary>
    ///   Retrievs the orderline of the given order id.
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns>collection of orderlines.</returns>
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

    /// <summary>
    ///   Updates the details of the specified order.
    /// </summary>
    /// <param name="request"></param>
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

    /// <summary>
    ///   Removes an order and all its subsequent orderline reccords.
    /// </summary>
    /// <param name="orderId"></param>
    public void DeleteOrder(Guid orderId)
    {
      using (var context = new SalesContext())
      {
        context.Materials.AsNoTracking().Load();
        context.Items.AsNoTracking().Load();
        context.OrderLines.Load();
        context.Clients.AsNoTracking().Load();

        var order = context.Orders.Include(x => x.OrderLines).SingleOrDefault(x => x.Id == orderId);
        if (order == default(Order))
        {
          throw new Exception($"The specified order {orderId} does not exist.");
        }

        context.Orders.Remove(order);
        context.SaveChanges();
      }
    }
  }
}