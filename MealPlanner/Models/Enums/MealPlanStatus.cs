using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Enums;

public enum MealPlanStatus
{
    [Display(Name = "Ej påbörjad")]
    NotStarted,

    [Display(Name = "Påbörjad")]
    InProgress,

    [Display(Name = "Avslutad")]
    Completed
}
