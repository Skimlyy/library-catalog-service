using Library_Catalog_Service.Data;
using Library_Catalog_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Catalog_Service.DTOs;

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
    public async Task<ActionResult<List<ItemDto>>> GetAll()
    {
        var items = await _db.Items
            .Include(x => x.ItemType)
            .AsNoTracking()
            .ToListAsync();

        var result = items.Select(x => new ItemDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Identifier = x.Identifier,
            IsActive = x.IsActive,
            ItemType = x.ItemType!.Name
        });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetById(int id)
    {
        var item = await _db.Items
            .Include(x => x.ItemType)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (item is null) return NotFound();

        var dto = new ItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Identifier = item.Identifier,
            IsActive = item.IsActive,
            ItemType = item.ItemType!.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(Item item)
    {
        var typeExists = await _db.ItemTypes.AnyAsync(x => x.Id == item.ItemTypeId);
        if (!typeExists) return BadRequest("ItemTypeId finns inte.");

        // Dublettskydd
        if (!string.IsNullOrWhiteSpace(item.Identifier))
        {
            var existing = await _db.Items
                .Include(x => x.ItemType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Identifier == item.Identifier);

            if (existing != null)
                return Conflict($"Item med identifier '{existing.Identifier}' finns redan: '{existing.Title}' ({existing.ItemType?.Name}).");
        }
        else
        {
            var existing = await _db.Items
                .Include(x => x.ItemType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.ItemTypeId == item.ItemTypeId &&
                    x.Title.ToLower() == item.Title.ToLower());

            if (existing != null)
                return Conflict($"'{existing.Title}' finns redan i kategorin '{existing.ItemType?.Name}'.");
        }

        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        
        var created = await _db.Items
            .Include(x => x.ItemType)
            .AsNoTracking()
            .FirstAsync(x => x.Id == item.Id);

        var dto = new ItemDto
        {
            Id = created.Id,
            Title = created.Title,
            Description = created.Description,
            Identifier = created.Identifier,
            IsActive = created.IsActive,
            ItemType = created.ItemType!.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Item item)
    {
        if (id != item.Id) return BadRequest("Id i URL matchar inte objektet.");

        var exists = await _db.Items.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        var typeExists = await _db.ItemTypes.AnyAsync(x => x.Id == item.ItemTypeId);
        if (!typeExists) return BadRequest("ItemTypeId finns inte.");

        // Dublettskydd
        if (!string.IsNullOrWhiteSpace(item.Identifier))
        {
            var existing = await _db.Items
                .Include(x => x.ItemType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id != id && x.Identifier == item.Identifier);

            if (existing != null)
                return Conflict($"Item med identifier '{existing.Identifier}' finns redan: '{existing.Title}' ({existing.ItemType?.Name}).");
        }
        else
        {
            var existing = await _db.Items
                .Include(x => x.ItemType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id != id &&
                    x.ItemTypeId == item.ItemTypeId &&
                    x.Title.ToLower() == item.Title.ToLower());

            if (existing != null)
                return Conflict($"'{existing.Title}' finns redan i kategorin '{existing.ItemType?.Name}'.");
        }

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