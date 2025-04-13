using MealPlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Models.Enums;
using MealPlanner.Models.ViewModel;
using MealPlanner.Services;


namespace MealPlanner.Controllers;

[Authorize]
public class MealPlansController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly MealPlanService _mealPlanService;

    public MealPlansController(ApplicationDbContext context, MealPlanService mealPlanService)
    {
        _context = context;
        _mealPlanService = mealPlanService;
    }

    public async Task<IActionResult> Index(MealPlanStatus? status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var mealPlans = await _mealPlanService.GetMealPlansByStatusAsync(userId, status);
        return View(mealPlans);
    }


    // GET: MealPlans/Create
    public async Task<IActionResult> Create()
    {
        var availableMeals = await _mealPlanService.GetAllMealSelectionsAsync();

        var model = new MealPlanCreateViewModel
        {
            Meals = availableMeals
        };

        return View(model);
    }

    // POST: MealPlans/Create
    [HttpPost]
    public async Task<IActionResult> Create(MealPlanCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Meals = await _mealPlanService.GetAllMealSelectionsAsync();
            return View(model);
        }

        if (!model.Meals.Any(m => m.Selected))
        {
            ModelState.AddModelError("", "Du måste välja minst en måltid.");
            model.Meals = await _mealPlanService.GetAllMealSelectionsAsync(); 
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var success = await _mealPlanService.CreateMealPlanAsync(model, userId);
        if (!success)
        {
            ModelState.AddModelError("", "Kunde inte skapa måltidsplanen.");
            model.Meals = await _mealPlanService.GetAllMealSelectionsAsync();
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: MealPlans/Edit
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var mealPlan = await _mealPlanService.GetMealPlanForUserAsync(id, userId);
        if (mealPlan == null) return NotFound();

        var availableMeals = await _mealPlanService.GetAvailableMealsForUserAsync(userId);

        var viewModel = new MealPlanEditViewModel
        {
            Id = mealPlan.Id,
            Title = mealPlan.Title,
            StartDate = mealPlan.StartDate,
            EndDate = mealPlan.EndDate,
            Status = mealPlan.Status,
            SelectedMealIds = mealPlan.Meals.Select(m => m.Id).ToList(),
            AvailableMeals = availableMeals
        };

        return View(viewModel);
    }

    // POST: MealPlans/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, MealPlanEditViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        if (!ModelState.IsValid || !model.SelectedMealIds.Any())
        {
            if (!model.SelectedMealIds.Any())
                ModelState.AddModelError("", "Du måste välja minst en måltid.");

            model.AvailableMeals = await _mealPlanService.GetAvailableMealsForUserAsync(userId);
            return View(model);
        }

        var success = await _mealPlanService.UpdateMealPlanAsync(model, userId);
        if (!success)
        {
            ModelState.AddModelError("", "Uppdatering av måltidsplanen misslyckades.");
            model.AvailableMeals = await _mealPlanService.GetAvailableMealsForUserAsync(userId);
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: MealPlans/Delete
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var mealPlan = await _mealPlanService.GetMealPlanForUserByIdAsync(id, userId);
        if (mealPlan == null)
            return NotFound();

        return View(mealPlan);
    }

    // POST: MealPlans/Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var success = await _mealPlanService.DeleteMealPlanAsync(id, userId);
        if (!success)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
