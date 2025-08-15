using Acme.Shop.Contracts.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Products
{
    public sealed class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
