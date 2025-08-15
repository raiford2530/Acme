using Acme.Shop.Contracts.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Products
{
    public sealed class ProductListRequestValidator : AbstractValidator<ProductListRequest>
    {
        private const int MaxPageSize = 100;

        public ProductListRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(MaxPageSize);
            RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue);
            RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue);
            RuleFor(x => x).Must(x => !(x.MinPrice.HasValue && x.MaxPrice.HasValue) || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice must be <= MaxPrice.");

            RuleFor(x => x.SortBy)
                .Must(s => s is null or "createdAt" or "price" or "name" or "sku")
                .WithMessage("SortBy must be one of: createdAt, price, name, sku.");
        }
    }
}
