namespace Product.Infrastructure.Repositories;

public interface IProductRepository : IBaseRepository<ProductEntity>
{
    Task<ProductEntity> GetByIdAsync(int id);
    Task<IEnumerable<ProductEntity>> GetAsync(string name = null, decimal? startPrice = null, decimal? endPrice = null);
    Task<IEnumerable<ProductEntity>> GetPaginatedAsync(int pageIndex, int pageSize = 10, string name = null, decimal? startPrice = null, decimal? endPrice = null);
    Task<bool> DeleteByIdAsync(int id);
}


public class ProductRepository : BaseRepository<ProductContext, ProductEntity>, IProductRepository
{
    public ProductRepository(ProductContext context) 
        : base(context)
    {
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var product = await GetByIdAsync(id);
        Remove(product);

        return true;
    }

    public async Task<IEnumerable<ProductEntity>> GetAsync(string name = null, decimal? startPrice = null, decimal? endPrice = null)
    {
        var queryable = FilteredProductQueryable(name, startPrice, endPrice);

        var products = await queryable
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return products;
    }

    public async Task<ProductEntity> GetByIdAsync(int id)
    {
        var product = await Entities
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Product by Id not found");

        return product;
    }

    public async Task<IEnumerable<ProductEntity>> GetPaginatedAsync(int pageIndex, int pageSize = 10, string name = null, decimal? startPrice = null, decimal? endPrice = null)
    {
        var queryable = FilteredProductQueryable(name, startPrice, endPrice);

        var products = await queryable
            .OrderBy(x => x.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return products;
    }

    private IQueryable<ProductEntity> FilteredProductQueryable(string name = null, decimal? startPrice = null, decimal? endPrice = null)
    {
        var predicates = new List<Expression<Func<ProductEntity, bool>>>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            predicates.Add(x => x.Name.Contains(name));
        }

        if (startPrice != null)
        {
            predicates.Add(x => x.Price >= startPrice.Value);

            if (endPrice != null)
            {
                predicates.Add(x => x.Price <= endPrice.Value);
            }
        }

        IQueryable<ProductEntity> products = Entities;

        foreach (var predicate in predicates)
        {
            products = products.Where(predicate);
        }

        return products;
    }
}
