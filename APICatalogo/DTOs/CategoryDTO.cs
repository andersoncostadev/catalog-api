using System.Text.Json.Serialization;

namespace APICatalogo.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    
    public string? Name { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [JsonIgnore]
    public ICollection<ProductDTO>? Products { get; set; }
}