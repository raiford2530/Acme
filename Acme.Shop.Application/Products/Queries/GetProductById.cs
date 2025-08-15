using Acme.Shop.Contracts.Products;
using Acme.Shop.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Products.Queries
{
    public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

    public sealed class GetProductByIdHandler(AppDbContext db) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
    {
        public async Task<ProductResponse?> Handle(GetProductByIdQuery q, CancellationToken ct)
        {
            var p = await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == q.Id, ct);
            return p is null ? null : new ProductResponse(p.Id, p.Sku, p.Name, p.Price, p.CreatedAtUtc);
        }
    }
}
