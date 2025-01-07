namespace Product.Application.Queries.ProductQuery;

public class GetProductByIdQuery : IRequest<ProductEntity>
{
    public int Id { get; set; }
}
