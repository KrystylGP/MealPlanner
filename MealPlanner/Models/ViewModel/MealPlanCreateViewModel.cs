using System.ComponentModel.DataAnnotations;
using MealPlanner.Models.Enums;

namespace MealPlanner.Models.ViewModel
{
    public class MealPlanCreateViewModel
    {
        [Required(ErrorMessage = "Titel krävs.")]
        [StringLength(100, ErrorMessage = "Titeln får vara max 100 tecken.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Startdatum krävs.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Slutdatum krävs.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Status krävs.")]
        public MealPlanStatus Status { get; set; }

        public List<MealSelectionViewModel> Meals { get; set; } = new();
    }
}
