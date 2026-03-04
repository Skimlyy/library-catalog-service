using Library_Catalog_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Catalog_Service.Data;

public static class CatalogSeeder
{
    public static async Task SeedAsync(CatalogDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.ItemTypes.AnyAsync())
        {
            db.ItemTypes.AddRange(
                new ItemType { Name = "Bok" },
                new ItemType { Name = "Tidning" },
                new ItemType { Name = "Utrustning" }
            );

            await db.SaveChangesAsync();
        }
    }
}