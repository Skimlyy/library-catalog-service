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
        var name = itemType.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Namn är obligatoriskt.");

        // Dublettskydd (case-insensitive)
        var existing = await _db.ItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

        if (existing != null)
            return Conflict($"Kategorin '{existing.Name}' finns redan (Id: {existing.Id}).");

        itemType.Name = name;

        _db.ItemTypes.Add(itemType);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = itemType.Id }, itemType);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ItemType itemType)
    {
        if (id != itemType.Id) return BadRequest("Id i URL matchar inte objektet.");

        var name = itemType.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name är obligatoriskt.");

        var exists = await _db.ItemTypes.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        // Dublettskydd (case-insensitive)
        var duplicate = await _db.ItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());

        if (duplicate != null)
            return Conflict($"Kategorin '{duplicate.Name}' finns redan (Id: {duplicate.Id}).");

        itemType.Name = name;

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