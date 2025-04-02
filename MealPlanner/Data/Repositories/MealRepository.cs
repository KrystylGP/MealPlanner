using MealPlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data.Repositories;

public class MealRepository : BaseRepository<Meal>
{
    public MealRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Meal>> GetAllWithIngredientsAsync()
    {
        return await _dbSet.Include(m => m.Ingredients).ToListAsync();
    }

    public async Task<Meal?> GetByIdWithIngredientsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

}
