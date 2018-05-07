using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
      using (var context = new SalesContext())
      {
        context.Materials.Load();
        context.Items.Load();
        var orders = await context.Orders
                                  .Include(x => x.Client)
                                  .Include(x => x.OrderLines)
                                  .ToListAsync();
        return orders;
      }
    }

    /// <summary>
    ///   Creates an order from the request parameters and adds it to the database.
    /// </summary>
    /// <param name="request"></param>
    public async Task AddOrderAsync(OrderRequest request)
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

        request.OrderLineRequests.ToList().ForEach(async x =>
        {
          var orderLine = new OrderLine
          {
            Id = Guid.NewGuid(),
            Item = await items.SingleAsync(y => y.Id == x.Item.Id),
            Order = order,
            Quantity = x.Quantity
          };
          order.OrderLines.Add(orderLine);
          context.OrderLines.Add(orderLine);
        });

        context.Orders.Add(order);
        await context.SaveChangesAsync();
      }
    }

    /// <summary>
    ///   Retrievs the orderline of the given order id.
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns>collection of orderlines.</returns>
    public async Task<IEnumerable<OrderLine>> GetOrderLinesByOrderIdAsync(Guid orderId)
    {
      using (var context = new SalesContext())
      {
        await context.Materials.LoadAsync();
        await context.Items.LoadAsync();
        await context.OrderLines.LoadAsync();
        await context.Clients.LoadAsync();
        var orderLines = (await context.Orders.SingleAsync(x => x.Id == orderId)).OrderLines.ToList();
        return orderLines;
      }
    }

    /// <summary>
    ///   Updates the details of the specified order.
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateOrderAsync(OrderRequest request)
    {
      using (var context = new SalesContext())
      {
        await context.Materials.AsNoTracking().LoadAsync();
        await context.Items.AsNoTracking().LoadAsync();
        await context.OrderLines.LoadAsync();
        await context.Clients.AsNoTracking().LoadAsync();

        var items = context.Items;
        var order = await context.Orders.SingleAsync(x => x.Id == request.OrderId);
        var orderLines = order.OrderLines;

        request.OrderLineRequests.ToList().ForEach(async x =>
        {
          var orderLine = orderLines.SingleOrDefault(y => y.Id == x.OrderLineId);
          if (orderLine != default(OrderLine))
          {
            orderLine.Quantity = x.Quantity;
            orderLine.Item = await items.SingleAsync(k => k.Id == x.Item.Id);
          }
          else
          {
            orderLines.Add(new OrderLine
            {
              Id = x.OrderLineId != default(Guid) ? x.OrderLineId : Guid.NewGuid(),
              Quantity = x.Quantity,
              Item = await items.SingleAsync(k => k.Id == x.Item.Id) 
            });
          }
        });

        context.Orders.Update(order);
        await context.SaveChangesAsync();
      }
    }

    /// <summary>
    ///   Removes an order and all its subsequent orderline reccords.
    /// </summary>
    /// <param name="orderId"></param>
    public async Task DeleteOrderAsync(Guid orderId)
    {
      using (var context = new SalesContext())
      {
        await context.Materials.AsNoTracking().LoadAsync();
        await context.Items.AsNoTracking().LoadAsync();
        await context.OrderLines.LoadAsync();
        await context.Clients.AsNoTracking().LoadAsync();

        var order = await context.Orders.Include(x => x.OrderLines).SingleOrDefaultAsync(x => x.Id == orderId);
        if (order == default(Order))
        {
          throw new Exception($"The specified order {orderId} does not exist.");
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();
      }
    }
  }
}