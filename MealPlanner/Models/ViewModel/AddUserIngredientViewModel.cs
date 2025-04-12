using System.ComponentModel.DataAnnotations;
using MealPlanner.Data.Entities;

namespace MealPlanner.Models.ViewModel
{
    public class AddUserIngredientViewModel
    {
        [Required(ErrorMessage = "Du måste välja en ingrediens.")]
        [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig ingrediens.")]
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Ange en mängd.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mängden måste vara minst 1.")]
        public int Quantity { get; set; }

        public List<Ingredient>? AvailableIngredients { get; set; }
    }
}
