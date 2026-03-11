using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library_Catalog_Service.Models;

public class ItemType
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [JsonIgnore]
    public List<Item> Items { get; set; } = new();
}