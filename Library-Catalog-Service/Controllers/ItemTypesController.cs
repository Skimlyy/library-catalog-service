using Library_Catalog_Service.Data;
using Library_Catalog_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Catalog_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemTypesController : ControllerBase
{
    private readonly CatalogDbContext _db;

    public ItemTypesController(CatalogDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemType>>> GetAll()
        => await _db.ItemTypes.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemType>> GetById(int id)
    {
        var itemType = await _db.ItemTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return itemType is null ? NotFound() : Ok(itemType);
    }

    [HttpPost]
    public async Task<ActionResult<ItemType>> Create(ItemType itemType)
    {
        // Enkel validering
        var name = itemType.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name är obligatoriskt.");

        // Dublett-koll (case-insensitive)
        var exists = await _db.ItemTypes.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        if (exists)
            return Conflict("ItemType med detta namn finns redan.");

        itemType.Name = name;

        _db.ItemTypes.Add(itemType);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = itemType.Id }, itemType);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ItemType itemType)
    {
        if (id != itemType.Id) return BadRequest("Id i URL matchar inte objektet.");

        var exists = await _db.ItemTypes.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        _db.Entry(itemType).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var itemType = await _db.ItemTypes.FindAsync(id);
        if (itemType is null) return NotFound();

        _db.ItemTypes.Remove(itemType);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}