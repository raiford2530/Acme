using Acme.Shop.Application.Products;
using Acme.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
