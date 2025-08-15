using Acme.Shop.Application.Common.Errors;
using Acme.Shop.Application.Interfaces;
using Acme.Shop.Contracts.Products;
using Acme.Shop.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Acme.Shop.Application.Products
{
    public sealed record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductResponse>;

    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IValidator<CreateProductRequest> _validator;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(
            IValidator<CreateProductRequest> validator,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand cmd, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(cmd.Request, ct);

            if (await _productRepository.ExistsBySkuAsync(cmd.Request.Sku, ct))
                throw new ConflictException($"SKU '{cmd.Request.Sku}' already exists.");

            var entity = new Product(cmd.Request.Sku, cmd.Request.Name, cmd.Request.Price);
            await _productRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new ProductResponse(entity.Id, entity.Sku, entity.Name, entity.Price, entity.CreatedAtUtc);
        }
    }
}
