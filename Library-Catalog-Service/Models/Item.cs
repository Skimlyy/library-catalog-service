using System.ComponentModel.DataAnnotations;
namespace Library_Catalog_Service.Models;

public class Item
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "";
    
    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Identifier { get; set; }  // ISBN eller inventarienummer

    public bool IsActive { get; set; } = true;

    [Required]
    public int ItemTypeId { get; set; }
    public ItemType? ItemType { get; set; }
}