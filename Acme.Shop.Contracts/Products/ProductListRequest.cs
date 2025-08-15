using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Contracts.Products
{
    public sealed record ProductListRequest(
        string? Sku = null,
        string? Name = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        string? SortBy = null,
        bool Desc = true,
        int Page = 1,
        int PageSize = 20);
}
