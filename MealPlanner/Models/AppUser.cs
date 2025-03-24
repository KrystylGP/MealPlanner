using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MealPlanner.Models;

public class AppUser : IdentityUser
{
    public virtual ICollection<Meal> Meals { get; set; } = null!;
}
