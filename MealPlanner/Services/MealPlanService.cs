using MealPlanner.Data.Repositories;
using MealPlanner.Models.Enums;

namespace MealPlanner.Services;

public class MealPlanService
{
    private readonly MealPlanRepository _repository;

    public MealPlanService(MealPlanRepository repository)
    {
        _repository = repository;
    }

    public Task<List<MealPlan>> GetForUser(string userId) =>
        _repository.GetByUserAsync(userId);

    public Task<List<MealPlan>> GetForUserByStatus(string userId, MealPlanStatus status) =>
        _repository.GetByUserAndStatusAsync(userId, status);
}
