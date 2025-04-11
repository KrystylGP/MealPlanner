using MealPlanner.Data.Entities;
using MealPlanner.Models.Enums;

namespace MealPlanner.Models.ViewModel
{
    public class MealPlanEditViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MealPlanStatus Status { get; set; }

        public List<int> SelectedMealIds { get; set; } = new();

        public List<Meal> AvailableMeals { get; set; } = new();
    }
}
