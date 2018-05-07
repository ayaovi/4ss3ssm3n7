using System;
using System.Collections.Generic;
using SalesApi.Models;

namespace SalesApi.Persistence
{
  public interface ISalesRepository
  {
    IEnumerable<Order> GetOrders();

    void AddOrder(OrderRequest order);

    IEnumerable<OrderLine> GetOrderLinesByOrderId(Guid orderId);

    void UpdateOrder(OrderRequest request);

    void DeleteOrder(Guid orderId);
  }
}
