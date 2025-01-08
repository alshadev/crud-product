namespace Identity.API.Infrastructure.Repositories;

public interface IBaseRepository<T> where T : class
{    
    IUnitOfWork UnitOfWork { get; }
    IQueryable<T> Entities { get; }

    Task AddAsync(T entity);
    Task AddRangeAsync(params T[] entities);
    void Remove(T entity);
    void RemoveRange(params T[] entities);
}

public abstract class BaseRepository<T, TEntity>(T context)
    where T : DbContext, IUnitOfWork
    where TEntity : class
{
    protected readonly T _context = context;

    public IUnitOfWork UnitOfWork => _context;
    public IQueryable<TEntity> Entities => _context.Set<TEntity>();

    public async Task AddAsync(TEntity entity) => await _context.AddAsync(entity);
    public void Remove(TEntity entity) => _context.Remove(entity);
    public async Task AddRangeAsync(params TEntity[] entities) => await _context.AddRangeAsync(entities);
    public void RemoveRange(params TEntity[] entities) => _context.RemoveRange(entities);
}