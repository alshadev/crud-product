namespace Product.Application.Commands.ProductCommand;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}
