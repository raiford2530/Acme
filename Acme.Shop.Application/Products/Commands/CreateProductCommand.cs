using Acme.Shop.Contracts.Products;
using Acme.Shop.Domain.Entities;
using Acme.Shop.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Products.Commands
{
    public sealed record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductResponse>;

    public sealed class CreateProductHandler(IValidator<CreateProductRequest> validator, AppDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        public async Task<ProductResponse> Handle(CreateProductCommand cmd, CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(cmd.Request, ct);

            var entity = new Product(cmd.Request.Sku, cmd.Request.Name, cmd.Request.Price);
            dbContext.Products.Add(entity);
            await dbContext.SaveChangesAsync(ct);

            return new ProductResponse(entity.Id, entity.Sku, entity.Name, entity.Price, entity.CreatedAtUtc);
        }
    }
}
