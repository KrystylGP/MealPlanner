using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.ViewModel
{
    public class MealCreateViewModel
    {
        [Required(ErrorMessage = "Namn krävs.")]
        [StringLength(100, ErrorMessage = "Namnet får vara max 100 tecken.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tillagningstid krävs.")]
        [Range(1, 300, ErrorMessage = "Tillagningstiden måste vara mellan 1 och 300 minuter.")]
        public int CookingTime { get; set; }

        public List<IngredientSelection> Ingredients { get; set; } = new();
    }

    public class IngredientSelection
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public int Quantity { get; set; }
    }

}
