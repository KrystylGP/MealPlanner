using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MealPlanner.Data.Entities;
using MealPlanner.Services;

namespace MealPlanner.Controllers;

// Skyddar hela controllern, endast inloggade användare får åtkomst.
[Authorize]
public class MealsController : Controller
{
    private readonly MealService _mealService;

    public MealsController(MealService mealService)
    {
        _mealService = mealService;
    }

    // Visar användarens meals
    public async Task<IActionResult> Index()
    {
        var meals = await _mealService.GetAllMealsAsync();
        return View(meals);
    }

    // GET: Meals/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Meal meal)
    {
        if (ModelState.IsValid)
        {
            meal.Ingredients = meal.Ingredients
                .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                .ToList();

            await _mealService.AddMealsAsync(meal);
            return RedirectToAction("Index");
        }

        return View(meal);
    }

    //GET: Meals/Edit
    public async Task<IActionResult> Edit(int id)
    {
        var meal = await _mealService.GetMealByIdAsync(id);
        if (meal == null)
            return NotFound();

        return View(meal);
    }

    // POST: Meals/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Meal meal)
    {
        if (id != meal.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            var existingMeal = await _mealService.GetMealByIdAsync(id);
            if (existingMeal == null)
                return NotFound(); // Om måltiden inte finns, returnera 404.

            existingMeal.Name = meal.Name;
            existingMeal.CookingTime = meal.CookingTime;
            existingMeal.Ingredients = meal.Ingredients
                .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                .ToList();

            await _mealService.UpdateMealAsync(existingMeal); // Sparar uppdateringen i databasen.
            return RedirectToAction("Index");
        }

        return View(meal);
    }

    // GET: Meals/Delete
    public async Task<IActionResult> Delete(int id)
    {
        var meal = await _mealService.GetMealByIdAsync(id);
        if (meal == null)
            return NotFound();

        return View(meal);
    }

    // POST: Meals/Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var meal = await _mealService.GetMealByIdAsync(id);
        if (meal == null)
            return NotFound();

        await _mealService.DeleteMealAsync(meal);
        return RedirectToAction("Index");
    }
}
