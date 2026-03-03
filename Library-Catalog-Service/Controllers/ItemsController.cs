using Library_Catalog_Service.Data;
using Library_Catalog_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Catalog_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly CatalogDbContext _db;

    public ItemsController(CatalogDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Item>>> GetAll()
        => await _db.Items
            .Include(x => x.ItemType)
            .AsNoTracking()
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Item>> GetById(int id)
    {
        var item = await _db.Items
            .Include(x => x.ItemType)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Item>> Create(Item item)
    {
        var typeExists = await _db.ItemTypes.AnyAsync(x => x.Id == item.ItemTypeId);
        if (!typeExists) return BadRequest("ItemTypeId finns inte.");

        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Item item)
    {
        if (id != item.Id) return BadRequest("Id i URL matchar inte objektet.");

        var exists = await _db.Items.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        var typeExists = await _db.ItemTypes.AnyAsync(x => x.Id == item.ItemTypeId);
        if (!typeExists) return BadRequest("ItemTypeId finns inte.");

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null) return NotFound();

        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}