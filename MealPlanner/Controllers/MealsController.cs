using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MealPlanner.Data;
using MealPlanner.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers;

[Authorize] // Skyddar hela controllern, endast inloggade användare får åtkomst.
public class MealsController : Controller
{
    private readonly ApplicationDbContext _context;

    public MealsController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var  userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var meals = await _context.Meals
            .Where(m => m.UserId == userId)
            .ToListAsync();
        
        return View(meals);
    }
}
