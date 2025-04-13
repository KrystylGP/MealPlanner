using MealPlanner.Data.Entities;
using MealPlanner.Data.Repositories;
using MealPlanner.Models.Enums;
using MealPlanner.Models.ViewModel;

namespace MealPlanner.Services;

public class MealPlanService
{
    private readonly MealPlanRepository _mealPlanRepository;
    private readonly MealRepository _mealRepository;

    public MealPlanService(MealPlanRepository repository, MealRepository mealRepository)
    {
        _mealPlanRepository = repository;
        _mealRepository = mealRepository;
    }

    public Task<List<MealPlan>> GetMealPlansByStatusAsync(string userId, MealPlanStatus? status)
    {
        return _mealPlanRepository.GetMealPlansByUserIdAndStatusAsync(userId, status);
    }
    public async Task<bool> CreateMealPlanAsync(MealPlanCreateViewModel model, string userId)
    {
        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(userId))
            return false;

        var selectedMealIds = model.Meals
            .Where(m => m.Selected)
            .Select(m => m.MealId)
            .ToList();

        var mealPlan = new MealPlan
        {
            Title = model.Title,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = model.Status,
            UserId = userId,
            Meals = selectedMealIds.Select(id => new Meal { Id = id }).ToList()
        };

        return await _mealPlanRepository.AddAsync(mealPlan);
    }

    public async Task<List<MealSelectionViewModel>> GetAllMealSelectionsAsync()
    {
        var meals = await _mealRepository.GetAllAsync();
        return meals.Select(m => new MealSelectionViewModel
        {
            MealId = m.Id,
            Name = m.Name,
            Selected = false
        }).ToList();
    }

    public async Task<MealPlan?> GetMealPlanForUserAsync(int mealPlanId, string userId)
    {
        return await _mealPlanRepository.GetMealPlanByIdAndUserAsync(mealPlanId, userId);
    }

    public async Task<List<Meal>> GetAvailableMealsForUserAsync(string userId)
    {
        return await _mealPlanRepository.GetAvailableMealsByUserIdAsync(userId);
    }

    public async Task<bool> UpdateMealPlanAsync(MealPlanEditViewModel model, string userId)
    {
        var mealPlan = await _mealPlanRepository.GetMealPlanByIdAndUserAsync(model.Id, userId);
        if (mealPlan == null)
            return false;

        mealPlan.Title = model.Title;
        mealPlan.StartDate = model.StartDate;
        mealPlan.EndDate = model.EndDate;
        mealPlan.Status = model.Status;

        var selectedMeals = await _mealPlanRepository.GetMealsByIdsAsync(model.SelectedMealIds);
        mealPlan.Meals.Clear();
        foreach (var meal in selectedMeals)
        {
            mealPlan.Meals.Add(meal);
        }

        return await _mealPlanRepository.UpdateMealPlanAsync(mealPlan);
    }

    public Task<MealPlan?> GetMealPlanForUserByIdAsync(int id, string userId)
    {
        return _mealPlanRepository.GetMealPlanForUserByIdAsync(id, userId);
    }

    public async Task<bool> DeleteMealPlanAsync(int id, string userId)
    {
        var mealPlan = await _mealPlanRepository.GetMealPlanForUserByIdAsync(id, userId);
        if (mealPlan == null)
            return false;

        return await _mealPlanRepository.DeleteAsync(mealPlan);
    }
}
