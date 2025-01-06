namespace Product.Domain.AggregateModels.ProductAggregate;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Product(string name, decimal price)
    {
        SetName(name);
        SetPrice(price);

        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price)
    {
        SetName(name);
        SetDescription(description);
        SetPrice(price);
    }

    public void SetName(string name) => Name = string.IsNullOrWhiteSpace(name) ? throw new Exception("Name is mandatory") : name.Trim();
    public void SetDescription(string description) => Description = description.Trim();
    public void SetPrice(decimal price) => Price = price <= 0 ? throw new Exception("Price should be greater than 0") : price;
}