namespace Library_Catalog_Service.Models;

public class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public string? Identifier { get; set; }  // ISBN eller inventarienummer

    public bool IsActive { get; set; } = true;

    public int ItemTypeId { get; set; }
    public ItemType? ItemType { get; set; }
}