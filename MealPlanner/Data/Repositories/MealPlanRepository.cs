using MealPlanner.Data.Entities;
using MealPlanner.Models.Enums;
using MealPlanner.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data.Repositories;

public class MealPlanRepository : BaseRepository<MealPlan>
{
    public MealPlanRepository(ApplicationDbContext context) : base(context){}

    public async Task<List<MealPlan>> GetMealPlansByUserIdAsync(string userId)
    {
        return await _context.MealPlans
            .Where(mp => mp.UserId == userId)
            .Include(mp => mp.Meals)
                .ThenInclude(m => m.MealIngredients)
                    .ThenInclude(mi => mi.Ingredient)
            .ToListAsync();
    }

    public Task<List<MealPlan>> GetMealPlansByUserIdAndStatusAsync(string userId, MealPlanStatus? status)
    {
        var query = _context.MealPlans
            .Where(mp => mp.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(mp => mp.Status == status.Value);
        }

        return query
            .Include(mp => mp.Meals)
            .ToListAsync();
    }

    public async Task<bool> AddAsync(MealPlan mealPlan)
    {
        try
        {
            foreach (var meal in mealPlan.Meals)
            {
                _context.Attach(meal);
            }

            _context.MealPlans.Add(mealPlan);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<MealPlan?> GetMealPlanByIdAndUserAsync(int mealPlanId, string userId)
    {
        return await _context.MealPlans
            .Include(mp => mp.Meals)
            .FirstOrDefaultAsync(mp => mp.Id == mealPlanId && mp.UserId == userId);
    }

    public async Task<List<Meal>> GetAvailableMealsByUserIdAsync(string userId)
    {
        return await _context.Meals
            .Where(m => m.MealPlans.Any(mp => mp.UserId == userId) || m.MealPlans.Count == 0)
            .ToListAsync();
    }

    public async Task<List<Meal>> GetMealsByIdsAsync(List<int> mealIds)
    {
        return await _context.Meals
            .Where(m => mealIds.Contains(m.Id))
            .ToListAsync();
    }

    public async Task<bool> UpdateMealPlanAsync(MealPlan mealPlan)
    {
        try
        {
            _context.MealPlans.Update(mealPlan);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public Task<MealPlan?> GetMealPlanForUserByIdAsync(int id, string userId)
    {
        return _context.MealPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && mp.UserId == userId);
    }

    public async Task<bool> DeleteAsync(MealPlan mealPlan)
    {
        try
        {
            _context.MealPlans.Remove(mealPlan);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

}