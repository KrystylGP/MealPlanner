using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MealPlanner.Models.Enums;

namespace MealPlanner.Data.Entities;

public class Ingredient
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar(24)")] // Sparas som text i databasen
    public IngredientType Type { get; set; }

    public ICollection<MealIngredient> MealIngredients { get; set; } = new List<MealIngredient>();
    public ICollection<UserIngredient> UserIngredients { get; set; } = new List<UserIngredient>();

}
