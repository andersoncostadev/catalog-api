using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Domain;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength (300)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(8,2)")]
    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime RegisterDate { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
}
