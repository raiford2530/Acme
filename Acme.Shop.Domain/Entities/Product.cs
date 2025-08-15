using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        [MaxLength(64)]
        public string Sku { get; private set; } = default!;
        [Required]
        [MaxLength(256)]
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

        private Product() { }

        public Product(string sku, string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("SKU required");
            }


            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name required");
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            Sku = sku.Trim();
            Name = name.Trim();
            Price = price;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name required");

            Name = name.Trim();
        }

        public void Reprice(decimal price)
        {
            if (price < 0) 
                throw new ArgumentOutOfRangeException(nameof(price));

            Price = price;
        }

    }
}
