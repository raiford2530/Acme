namespace Acme.Shop.Contracts.Products
{
    public sealed record ProductQueryParameters(
        string? Sku,
        string? Name,
        decimal? MinPrice,
        decimal? MaxPrice,
        string SortBy,  // normalized, default "createdAt"
        bool Desc,
        int Page,
        int PageSize);
}
