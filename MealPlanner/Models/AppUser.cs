using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}
