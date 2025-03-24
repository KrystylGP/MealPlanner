using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Models;

public class Meal
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Ingredients { get; set; } = null!;
    public string Status { get; set; } = null!;

    // Koppling till användare
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; } = null!;

}
