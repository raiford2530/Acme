using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Contracts.Products
{
    public sealed record CreateProductRequest(string Sku, string Name, decimal Price);
}
