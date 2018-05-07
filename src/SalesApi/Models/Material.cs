using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
  public class Material
  {
    /// <summary>
    ///   A unique way of identifying a material.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    ///   More information on the making of the material.
    /// </summary>
    public string Description { get; set; }
  }
}