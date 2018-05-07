using System;
using System.Collections.Generic;

namespace SalesApi.Models
{
  public class Order
  {
    /// <summary>
    ///   A unique wa of identifying an order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///   The date this order is placed.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///   The client to whom this order belongs to.
    /// </summary>
    public virtual Client Client { get; set; }

    /// <summary>
    ///   An order is made up of one or multiple lines.
    /// </summary>
    public virtual ICollection<OrderLine> OrderLines { get; set; }
  }
}