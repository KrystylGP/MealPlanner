using System.ComponentModel.DataAnnotations;
using MealPlanner.Models.Enums;
using MealPlanner.Models;

namespace MealPlanner.Data.Entities;

public class Ingredient
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public IngredientType Type { get; set; }

    public int MealId { get; set; }
    public Meal Meal { get; set; } = null!;

}
