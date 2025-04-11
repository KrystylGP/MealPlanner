using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Data.Entities;

public class Meal
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public int CookingTime { get; set; }


    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
    public ICollection<MealIngredient> MealIngredients { get; set; } = new List<MealIngredient>();

}
