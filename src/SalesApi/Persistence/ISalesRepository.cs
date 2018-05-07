using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApi.Models;

namespace SalesApi.Persistence
{
  public interface ISalesRepository
  {
    Task<IEnumerable<Order>> GetOrdersAsync();

    Task AddOrderAsync(OrderRequest order);

    Task<IEnumerable<OrderLine>> GetOrderLinesByOrderIdAsync(Guid orderId);

    Task UpdateOrderAsync(OrderRequest request);

    Task DeleteOrderAsync(Guid orderId);
  }
}
