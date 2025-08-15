using Acme.Shop.Application.Products;
using Acme.Shop.Contracts.Products;
using Acme.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Infrastructure.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Product product, CancellationToken ct = default) =>
            await _dbContext.Products.AddAsync(product, ct);

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default) =>
            await _dbContext.Products.AsNoTracking().AnyAsync(p => p.Sku == sku, ct);

        public void Remove(Product product) => _dbContext.Products.Remove(product);

        public async Task<(IReadOnlyList<Product> Items, int TotalCount)> SearchAsync(
        ProductQueryParameters c, CancellationToken ct = default)
        {
            var query = _dbContext.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(c.Sku))
            {
                var sku = c.Sku.ToLower().Trim();
                query = query.Where(p => p.Sku.ToLower().Contains(sku));
            }

            if (!string.IsNullOrWhiteSpace(c.Name))
            {
                var name = c.Name.ToLower().Trim();
                query = query.Where(p => p.Name.ToLower().Contains(name));
            }

            if (c.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= c.MinPrice.Value);
            }

            if (c.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= c.MaxPrice.Value);
            }

            // Sorting
            Expression<Func<Product, object>> keySelector = c.SortBy switch
            {
                "price" => p => p.Price,
                "name" => p => p.Name,
                "sku" => p => p.Sku,
                _ => p => p.CreatedAtUtc, // "createdAt"
            };

            query = c.Desc ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);

            // Total count before paging
            var total = await query.CountAsync(ct);

            var items = await query
                .Skip((c.Page - 1) * c.PageSize)
                .Take(c.PageSize)
                .ToListAsync(ct);

            return (items, total);
        }
    }
}
