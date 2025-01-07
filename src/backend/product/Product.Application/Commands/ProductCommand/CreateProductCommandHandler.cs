namespace Product.Application.Commands.ProductCommand;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new ProductEntity(request.Name, request.Price);
        newProduct.SetDescription(request.Description);
        
        await _productRepository.AddAsync(newProduct);
        await _productRepository.UnitOfWork.SaveChangesAsync();

        return newProduct.Id;
    }
}