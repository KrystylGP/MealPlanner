using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Enums;

public enum IngredientType
{
    [Display(Name = "Grönsak")]
    Vegetable,

    [Display(Name = "Frukt")]
    Fruit,

    [Display(Name = "Kött")]
    Meat,

    [Display(Name = "Fisk")]
    Fish,

    [Display(Name = "Mejeri")]
    Dairy,

    [Display(Name = "Kolhydrat")]
    Carb,

    [Display(Name = "Krydda")]
    Spice,

    [Display(Name = "Annat")]
    Other
}
