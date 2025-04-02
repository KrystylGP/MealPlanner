using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Data.Entities;

public class Meal
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public int CookingTime { get; set; }


    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

}
