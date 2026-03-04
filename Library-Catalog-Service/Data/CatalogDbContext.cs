using Library_Catalog_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Catalog_Service.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemType> ItemTypes => Set<ItemType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemType>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<Item>()
            .HasIndex(x => x.Identifier)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}