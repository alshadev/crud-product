
namespace Product.Application.Queries.ProductQuery;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductEntity>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ProductEntity> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(request.Id);
    }
}
