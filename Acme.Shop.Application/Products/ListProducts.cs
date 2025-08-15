using Acme.Shop.Contracts.Common;
using Acme.Shop.Contracts.Products;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Products
{
    public sealed record ListProductsQuery(ProductListRequest Request)
    : IRequest<PagedResponse<ProductResponse>>;

    public sealed class ListProductsHandler(
        IValidator<ProductListRequest> validator,
        IProductRepository productRepository)
        : IRequestHandler<ListProductsQuery, PagedResponse<ProductResponse>>
    {
        public async Task<PagedResponse<ProductResponse>> Handle(ListProductsQuery q, CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(q.Request, ct);

            var sortBy = (q.Request.SortBy ?? "createdAt").ToLowerInvariant();

            var criteria = new ProductQueryParameters(
                q.Request.Sku,
                q.Request.Name,
                q.Request.MinPrice,
                q.Request.MaxPrice,
                sortBy,
                q.Request.Desc,
                q.Request.Page,
                q.Request.PageSize);

            var (items, total) = await productRepository.SearchAsync(criteria, ct);

            var projected = items
                .Select(p => new ProductResponse(p.Id, p.Sku, p.Name, p.Price, p.CreatedAtUtc))
                .ToList();

            return new PagedResponse<ProductResponse>(projected, q.Request.Page, q.Request.PageSize, total);
        }
    }
}
