using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
  public class Client
  {
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    /// <summary>
    ///   A unique way of identifying a client.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    ///   A client can have multiple orders in their name.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; }
  }
}