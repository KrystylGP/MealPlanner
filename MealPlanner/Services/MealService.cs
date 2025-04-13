using MealPlanner.Data.Entities;
using MealPlanner.Data.Repositories;
using MealPlanner.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class MealService
    {
        private readonly MealRepository _repository;

        public MealService(MealRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Meal>> GetAllMealsAsync() =>
            _repository.GetAllAsync().ContinueWith(t => t.Result.ToList());

        public Task<Meal?> GetMealByIdAsync(int id) =>
            _repository.GetAsync(m => m.Id == id);

        public Task<List<IngredientSelection>> GetIngredientSelectionsAsync()
        {
            return _repository.GetIngredientSelectionsAsync();
        }

        public Task<bool> CreateMealAsync(MealCreateViewModel model)
        {
            return _repository.CreateMealFromViewModelAsync(model);
        }

        public Task<bool> UpdateMealAsync(Meal meal) =>
            _repository.UpdateAsync(meal);

        public Task<bool> DeleteMealAsync(Meal meal) =>
            _repository.DeleteAsync(meal);

        public Task<Meal?> GetMealWithIngredientsByIdAsync(int id) =>
            _repository.GetMealWithIngredientsByIdAsync(id);

        public List<IngredientSelection> GetAllIngredientsForMealCreate()
        {
            return _repository.GetAllIngredientsAsync().Result
                .Select(i => new IngredientSelection { IngredientId = i.Id, Name = i.Name })
                .ToList();
        }
    }
}
