namespace Product.Application.Queries.ProductQuery;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<ProductEntity>>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductEntity>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAsync(request.Name, request.StartPrice, request.EndPrice);
        return products.ToList();
    }
}