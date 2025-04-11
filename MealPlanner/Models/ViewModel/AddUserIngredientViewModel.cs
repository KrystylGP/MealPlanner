using MealPlanner.Data.Entities;

namespace MealPlanner.Models.ViewModel
{
    public class AddUserIngredientViewModel
    {
        public int IngredientId { get; set; }
        public int Quantity { get; set; }

        public List<Ingredient>? AvailableIngredients { get; set; }
    }
}
