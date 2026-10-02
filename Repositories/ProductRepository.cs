using Microsoft.EntityFrameworkCore;
using ProductMaintenance.Data;
using ProductMaintenance.Models;

namespace ProductMaintenance.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<bool> CheckExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Products
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> CheckExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context
            .Products
            .AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<bool> CheckNameExistsAsync(int id, string name, CancellationToken cancellationToken = default)
    {
        return await _context
            .Products
            .AnyAsync(x => x.Id != id && x.Name == name, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _context
            .Products
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductListItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Select(x => new ProductListItem
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            })
            .OrderBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id,cancellationToken);
    } 

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);

        await _context.SaveChangesAsync(cancellationToken);
    }    
}
