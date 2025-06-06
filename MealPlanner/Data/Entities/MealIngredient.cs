﻿namespace MealPlanner.Data.Entities;

public class MealIngredient
{
    public int MealId { get; set; }
    public Meal Meal { get; set; } = null!;

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    public int Quantity { get; set; }
}
