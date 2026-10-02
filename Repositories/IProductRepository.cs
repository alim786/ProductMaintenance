using ProductMaintenance.Models;

namespace ProductMaintenance.Repositories;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);

    Task<bool> CheckExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> CheckExistsAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> CheckNameExistsAsync(int id, string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductListItem>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
}
