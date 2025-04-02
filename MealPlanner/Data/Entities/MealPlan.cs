using MealPlanner.Data.Entities;
using MealPlanner.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class MealPlan
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MealPlanStatus Status { get; set; }


    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;


    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
