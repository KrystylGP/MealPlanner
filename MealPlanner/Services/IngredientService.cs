using MealPlanner.Data.Entities;
using MealPlanner.Data.Repositories;

namespace MealPlanner.Services;

public class IngredientService
{
    private readonly IngredientRepository _ingredientRepository;
    private readonly MealPlanRepository _mealPlanRepository;

    public IngredientService(IngredientRepository ingredientRepository, MealPlanRepository mealPlanRepository)
    {
        _ingredientRepository = ingredientRepository;
        _mealPlanRepository = mealPlanRepository;
    }

    public Task<List<UserIngredient>> GetUserIngredientsAsync(string userId)
    {
        return _ingredientRepository.GetUserIngredientsByUserIdAsync(userId);
    }

    public Task<List<Ingredient>> GetAllAsync()
    {
        return _ingredientRepository.GetAllIngredientsAsync();
    }

    public async Task AddUserIngredientAsync(string userId, int ingredientId, int quantity)
    {
        var existingUserIngredient = await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);

        if (existingUserIngredient != null)
        {
            existingUserIngredient.Quantity += quantity;
            await _ingredientRepository.UpdateUserIngredientAsync(existingUserIngredient);
        }
        else
        {
            var newUserIngredient = new UserIngredient
            {
                UserId = userId,
                IngredientId = ingredientId,
                Quantity = quantity
            };

            await _ingredientRepository.AddUserIngredientAsync(newUserIngredient);
        }
    }

    public async Task<Dictionary<string, int>> GenerateGroceryListAsync(string userId)
    {
        // Hämtar användarens ingredienser
        var userStock = await GetUserStockAsync(userId);

        // Hämtar användarens behövda ingredienser för måltidsplanen
        var plannedMealIngredients = await GetPlannedMealIngredientsAsync(userId);

        // Skapar listan med keys and values par ingredient <namn, antal>
        var groceryList = new Dictionary<string, int>();

        // Jämför "behövda" mot "har" ingredienser och kalkylerar
        foreach (var item in plannedMealIngredients)
        {
            var inStock = userStock.ContainsKey(item.Ingredient.Id) ? userStock[item.Ingredient.Id] : 0;
            var missing = item.Quantity - inStock;

            // Fyller listan med saknade ingredienser
            if (missing > 0)
                groceryList[item.Ingredient.Name] = missing;
        }

        return groceryList;
    }

    private async Task<Dictionary<int, int>> GetUserStockAsync(string userId)
    {
        var userStock = await _ingredientRepository.GetUserStockByUserIdAsync(userId);
        return userStock.ToDictionary(x => x.IngredientId, x => x.Quantity);
    }

    private async Task<List<MealIngredient>> GetPlannedMealIngredientsAsync(string userId)
    {
        var mealPlans = await _mealPlanRepository.GetMealPlansByUserIdAsync(userId);

        var mealIngredients = mealPlans
            .SelectMany(mp => mp.Meals)
            .SelectMany(m => m.MealIngredients)
            .GroupBy(mi => mi.Ingredient)
            .Select(g => new MealIngredient
            {
                Ingredient = g.Key,
                Quantity = g.Sum(mi => mi.Quantity)
            })
            .ToList();

        return mealIngredients;
    }

    public async Task<bool> DeleteUserIngredientAsync(string userId, int ingredientId)
    {
        var userIngredient = await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);

        if (userIngredient == null)
            return false;

        await _ingredientRepository.DeleteUserIngredientAsync(userIngredient);
        return true;
    }

    public async Task<bool> AddIngredientAsync(Ingredient ingredient)
    {
        if (ingredient == null) return false;

        var result = await _ingredientRepository.AddAsync(ingredient);
        return result;
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(int id)
    {
        return await _ingredientRepository.GetAsync(i => i.Id == id);
    }

    public async Task<bool> UpdateIngredientAsync(Ingredient ingredient)
    {
        if (ingredient == null)
            return false;

        var result = await _ingredientRepository.UpdateAsync(ingredient);
        return result;
    }

    public async Task<UserIngredient?> FetchUserIngredientAsync(string userId, int ingredientId)
    {
        return await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);
    }

    public async Task<bool> UpdateUserIngredientQuantityAsync(string userId, int ingredientId, int quantity)
    {
        var userIngredient = await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);
        if (userIngredient == null)
            return false;

        userIngredient.Quantity = quantity;
        return await _ingredientRepository.UpdateUserIngredientAsync(userIngredient);
    }
}
