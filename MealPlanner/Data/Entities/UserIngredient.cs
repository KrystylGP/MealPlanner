namespace MealPlanner.Data.Entities;

public class UserIngredient
{
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    public int Quantity { get; set; }
}
