namespace Library_Catalog_Service.DTOs;
public class ItemDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Identifier { get; set; }
    
    public bool IsActive { get; set; }

    public string ItemType { get; set; } = "";
}