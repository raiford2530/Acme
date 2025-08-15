namespace Acme.Shop.Contracts.Products
{
    public sealed record ProductResponse(Guid Id, string Sku, string Name, decimal Price, DateTimeOffset CreatedAtUtc);
}
