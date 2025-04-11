using MealPlanner.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data.Repositories;

public class MealPlanRepository : BaseRepository<MealPlan>
{
    public MealPlanRepository(ApplicationDbContext context) : base(context){}

    public async Task<List<MealPlan>> GetByUserAsync(string userId)
    {
        return await _dbSet
            .Where(mp => mp.UserId == userId)
            .Include(mp => mp.Meals)
            .ToListAsync();
    }

    public async Task<List<MealPlan>> GetByUserAndStatusAsync(string userId, MealPlanStatus status)
    {
        return await _dbSet
            .Where(mp => mp.UserId == userId && mp.Status == status)
            .Include(mp => mp.Meals)
            .ToListAsync();
    }

    public async Task<List<MealPlan>> GetMealPlansByUserIdAsync(string userId)
    {
        return await _context.MealPlans
            .Where(mp => mp.UserId == userId)
            .Include(mp => mp.Meals)
                .ThenInclude(m => m.MealIngredients)
                    .ThenInclude(mi => mi.Ingredient)
            .ToListAsync();
    }

}