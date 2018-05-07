using System;

namespace SalesApi.Models
{
  /// <summary>
  ///   For http request coming in. Requests only need to have the necessary property for carrying them out.
  /// </summary>
  public class OrderLineRequest
  {
    /// <summary>
    ///   Identifies the orderline in question.
    /// </summary>
    public Guid OrderLineId { get; set; }

    /// <summary>
    ///   Refers to the item in question.
    /// </summary>
    public Item Item { get; set; }

    /// <summary>
    ///   The number of item in the orderline.
    /// </summary>
    public int Quantity { get; set; }
  }
}