using Microsoft.EntityFrameworkCore;
using ProductMaintenance.Constants;
using ProductMaintenance.Models;

namespace ProductMaintenance.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Name).IsUnique();

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(DBConstants.PRODUCT_NAME_MAXLENGTH);

            entity.Property(p => p.Price)
                .HasPrecision(DBConstants.PRODUCT_PRICE_PRECISION, DBConstants.PRODUCT_PRICE_SCALE);

            entity.Property(p => p.Stock)
                .IsRequired();
        });
    }
}
