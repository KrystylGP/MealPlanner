using MealPlanner.Data.Entities;
using MealPlanner.Models.ViewModel;
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

        public Task<List<IngredientSelection>> GetIngredientSelectionsAsync()
        {
            return _context.Ingredients
                .Select(i => new IngredientSelection
                {
                    IngredientId = i.Id,
                    Name = i.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CreateMealFromViewModelAsync(MealCreateViewModel model)
        {
            try
            {
                var meal = new Meal
                {
                    Name = model.Name,
                    CookingTime = model.CookingTime,
                    MealIngredients = model.Ingredients
                        .Where(i => i.Selected && i.Quantity > 0)
                        .Select(i => new MealIngredient
                        {
                            IngredientId = i.IngredientId,
                            Quantity = i.Quantity
                        }).ToList()
                };

                _context.Meals.Add(meal);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
