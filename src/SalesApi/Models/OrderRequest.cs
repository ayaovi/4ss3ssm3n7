using System;
using System.Collections.Generic;

namespace SalesApi.Models
{
  public class OrderRequest
  {
    /// <summary>
    ///   Identifies the client to whom the order belongs to.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    ///   Unique identifie of an order in orders reccord.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    ///   An order has multiple order lines.
    /// </summary>
    public IEnumerable<OrderLineRequest> OrderLineRequests { get; set; }
  }
}
