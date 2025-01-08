namespace Product.Application.Queries.ProductQuery;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductEntity>
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _memoryCache;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMemoryCache memoryCache)
    {
        _productRepository = productRepository;
        _memoryCache = memoryCache;
    }
    
    public async Task<ProductEntity> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"product-{request.Id}";
        if (!_memoryCache.TryGetValue(cacheKey, out ProductEntity product))
        {
            product = await _productRepository.GetByIdAsync(request.Id);

            // Set cache with expiration time (60 seconds)
            _memoryCache.Set(cacheKey, product, TimeSpan.FromSeconds(60));
        }
        return product;
    }
}
