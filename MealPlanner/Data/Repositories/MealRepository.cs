using MealPlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealPlanner.Data.Repositories
{
    public class MealRepository : BaseRepository<Meal>
    {
        public MealRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Meal?> GetMealWithIngredientsByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.MealIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return _context.Ingredients.ToListAsync();
        }
    }
}
