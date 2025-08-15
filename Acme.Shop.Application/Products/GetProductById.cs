using Acme.Shop.Contracts.Products;
using MediatR;

namespace Acme.Shop.Application.Products
{
    public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

    public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponse?> Handle(GetProductByIdQuery q, CancellationToken ct)
        {
            var p = await _productRepository.GetByIdAsync(q.Id, ct);
            return p is null ? null : new ProductResponse(p.Id, p.Sku, p.Name, p.Price, p.CreatedAtUtc);
        }
    }
}
