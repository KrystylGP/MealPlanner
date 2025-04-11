using MealPlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MealPlanner.Data.Repositories;

public class IngredientRepository : BaseRepository<Ingredient>
{
    public IngredientRepository(ApplicationDbContext context) : base(context){ }

    public Task<List<UserIngredient>> GetUserIngredientsByUserIdAsync(string userId)
    {
        return _context.UserIngredients
            .Where(ui => ui.UserId == userId)
            .Include(ui => ui.Ingredient)
            .ToListAsync();
    }

    public Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        return _context.Ingredients.ToListAsync();
    }

    public Task<UserIngredient?> GetUserIngredientAsync(string userId, int ingredientId)
    {
        return _context.UserIngredients
            .Include(ui => ui.Ingredient)
            .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.IngredientId == ingredientId);
    }


    public Task AddUserIngredientAsync(UserIngredient userIngredient)
    {
        _context.UserIngredients.Add(userIngredient);
        return _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateUserIngredientAsync(UserIngredient userIngredient)
    {
        try
        {
            _context.UserIngredients.Update(userIngredient);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }


    public Task<List<UserIngredient>> GetUserStockByUserIdAsync(string userId)
    {
        return _context.UserIngredients
            .Where(ui => ui.UserId == userId)
            .Include(ui => ui.Ingredient)
            .ToListAsync();
    }

    public async Task DeleteUserIngredientAsync(UserIngredient userIngredient)
    {
        _context.UserIngredients.Remove(userIngredient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AddAsync(Ingredient ingredient)
    {
        try
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Get an ingredient by Id
    public async Task<Ingredient?> GetAsync(Expression<Func<Ingredient, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    // Update an ingredient
    public async Task<bool> UpdateAsync(Ingredient ingredient)
    {
        try
        {
            _dbSet.Update(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }


}
