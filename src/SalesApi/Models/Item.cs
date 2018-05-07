using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
  public class Item
  {
    /// <summary>
    ///   A unique way of identifying an item.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    ///   More information on what the item is.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   An item has a making material.
    /// </summary>
    public virtual Material Material { get; set; }
  }
}