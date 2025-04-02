using MealPlanner.Data.Entities;
using MealPlanner.Data.Repositories;

namespace MealPlanner.Services;

public class MealService
{
    private readonly MealRepository _repository;

    public MealService(MealRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Meal>> GetAllMealsAsync() =>
        _repository.GetAllWithIngredientsAsync();

    public Task<Meal?> GetMealByIdAsync(int id) =>
       _repository.GetByIdWithIngredientsAsync(id);

    public Task<bool> AddMealsAsync(Meal meal) =>
       _repository.AddAsync(meal);

    public Task<bool> UpdateMealAsync(Meal meal) =>
      _repository.AddAsync(meal);

    public Task<bool> DeleteMealAsync(Meal meal) =>
      _repository.AddAsync(meal);
}
