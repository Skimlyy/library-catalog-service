using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library_Catalog_Service.Data;

public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=catalog.db");

        return new CatalogDbContext(optionsBuilder.Options);
    }
}