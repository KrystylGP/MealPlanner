using System.ComponentModel.DataAnnotations;
using MealPlanner.Models.Enums;

namespace MealPlanner.Models.ViewModel
{
    public class IngredientCreateViewModel
    {
        [Required(ErrorMessage = "Namn krävs.")]
        [StringLength(100, ErrorMessage = "Namnet får vara max 100 tecken.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Typ krävs.")]
        public IngredientType Type { get; set; }
    }
}
