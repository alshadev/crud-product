namespace Product.Application.Commands.ProductCommand;

public class UpdateProductCommand : IRequest<bool>
{
    public int Id { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    public string Description { get; set; }
}
