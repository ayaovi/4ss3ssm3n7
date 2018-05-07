using System;

namespace SalesApi.Models
{
  public class OrderLine
  {
    /// <summary>
    ///   A unique way of identifying an orderline.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///   The number of item in this orderline.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///   An oderline refers to an item.
    /// </summary>
    public virtual Item Item { get; set; }

    /// <summary>
    ///   The order that this orderline is part of.
    /// </summary>
    public virtual Order Order { get; set; }
  }
}