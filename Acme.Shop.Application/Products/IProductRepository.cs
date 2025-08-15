using Acme.Shop.Contracts.Products;
using Acme.Shop.Domain.Entities;

namespace Acme.Shop.Application.Products
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default);
        void Remove(Product product);
        Task<(IReadOnlyList<Product> Items, int TotalCount)> SearchAsync(
        ProductQueryParameters criteria, CancellationToken ct = default);
    }
}
