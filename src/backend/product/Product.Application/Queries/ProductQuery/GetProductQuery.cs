namespace Product.Application.Queries.ProductQuery;

public class GetProductQuery : IRequest<List<ProductEntity>>
{
    public string Name { get; set; }
    public decimal? StartPrice { get; set; }
    public decimal? EndPrice { get; set; }
}