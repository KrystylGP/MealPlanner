using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MealPlanner.Data.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    // CRUD
    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        try
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    // Hämta en lista av entiteter (ej sorterad)
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return entities;
    }

    // Hämta en lista av entiteter (sorterad)
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entities = await _dbSet.Where(expression).ToListAsync();
        return entities;
    }

    // Hämta en entitet
    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(expression);
        return entity;
    }

    // Kontrollera om entitet existerar
    public virtual async Task<TEntity?> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        var exists = await _dbSet.FirstOrDefaultAsync(expression);
        return exists;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

}
