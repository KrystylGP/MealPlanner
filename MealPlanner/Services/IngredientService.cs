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
            // Update the quantity if the ingredient already exists for the user
            existingUserIngredient.Quantity += quantity;
            await _ingredientRepository.UpdateUserIngredientAsync(existingUserIngredient);
        }
        else
        {
            // If the ingredient doesn't exist for the user, add a new one
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
        // Fetch the user's stock of ingredients
        var userStock = await GetUserStockAsync(userId);

        // Fetch the planned meal ingredients
        var plannedMealIngredients = await GetPlannedMealIngredientsAsync(userId);

        // Compare the ingredients in stock with the needed ingredients
        var groceryList = new Dictionary<string, int>();

        // For each ingredient in the planned meals, calculate the missing amount
        foreach (var item in plannedMealIngredients)
        {
            // Sum all the quantities of the same ingredient in the meal plans
            var inStock = userStock.ContainsKey(item.Ingredient.Id) ? userStock[item.Ingredient.Id] : 0;
            var missing = item.Quantity - inStock; // item.Quantity is the sum of needed quantity for this ingredient

            // If the ingredient is missing, add it to the grocery list
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
                Quantity = g.Sum(mi => mi.Quantity)  // Total quantity needed
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

    // Add an ingredient
    public async Task<bool> AddIngredientAsync(Ingredient ingredient)
    {
        if (ingredient == null) return false;

        var result = await _ingredientRepository.AddAsync(ingredient);
        return result;
    }

    // Get an ingredient by Id
    public async Task<Ingredient?> GetIngredientByIdAsync(int id)
    {
        return await _ingredientRepository.GetAsync(i => i.Id == id);
    }

    // Update an ingredient
    public async Task<bool> UpdateIngredientAsync(Ingredient ingredient)
    {
        if (ingredient == null)
            return false;

        var result = await _ingredientRepository.UpdateAsync(ingredient);
        return result;
    }

    public async Task<UserIngredient?> FetchUserIngredientAsync(string userId, int ingredientId)
    {
        // This just calls the repository to get the user's ingredient.
        return await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);
    }

    public async Task<bool> UpdateUserIngredientQuantityAsync(string userId, int ingredientId, int quantity)
    {
        var userIngredient = await _ingredientRepository.GetUserIngredientAsync(userId, ingredientId);
        if (userIngredient == null)
            return false;

        // Update the quantity to the new value
        userIngredient.Quantity = quantity;
        return await _ingredientRepository.UpdateUserIngredientAsync(userIngredient);
    }


}
