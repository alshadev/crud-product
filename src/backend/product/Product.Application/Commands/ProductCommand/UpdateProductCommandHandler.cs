namespace Product.Application.Commands.ProductCommand;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _memoryCache;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMemoryCache memoryCache)
    {
        _productRepository = productRepository;
        _memoryCache = memoryCache;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        product.Update(request.Description, request.Price);
        
        _memoryCache.Remove($"product-{product.Id}");

        await _productRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }
}
