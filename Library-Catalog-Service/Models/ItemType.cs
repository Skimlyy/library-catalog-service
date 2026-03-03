using System.Text.Json.Serialization;

namespace Library_Catalog_Service.Models;

public class ItemType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public List<Item> Items { get; set; } = new();
}