using MealPlanner.Data.Entities;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
    public ICollection<UserIngredient> UserIngredients { get; set; } = new List<UserIngredient>();
}
