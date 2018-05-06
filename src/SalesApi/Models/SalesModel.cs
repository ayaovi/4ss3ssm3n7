using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
  public class Client
  {
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
  }

  public class Order
  {
    public Guid Id { get; set; }

    public virtual Client Client { get; set; }

    public virtual ICollection<OrderLine> OrderLines { get; set; }
  }

  public class OrderLine
  {
    public Guid Id { get; set; }

    public int Quantity { get; set; }

    //public Guid OrderId { get; set; }

    public virtual Item Item { get; set; }

    public virtual Order Order { get; set; }
  }

  public class Item
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Description { get; set; }

    public virtual Material Material { get; set; }
  }

  public class Material
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Description { get; set; }
  }

  public class OrderLineRequest
  {
    //public Guid OrderId { get; set; }

    public Item Item { get; set; }

    public int Quantity { get; set; }
  }

  public class OrderRequest
  {
    public int ClientId { get; set; }
    public IEnumerable<OrderLineRequest> OrderLineRequests { get; set; }
  }
}
