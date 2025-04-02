using MealPlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Models.Enums;


namespace MealPlanner.Controllers;

[Authorize]
public class MealPlansController : Controller
{
    private readonly ApplicationDbContext _context;

    public MealPlansController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Filtrera MealPlans baserat på status
    public async Task<IActionResult> Index(MealPlanStatus? status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var query = _context.MealPlans
            .Where(mp => mp.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(mp => mp.Status == status.Value);
        }

        var mealPlans = await query
            .Include(mp => mp.Meals)
            .ToListAsync();

        return View(mealPlans);
    }

    // GET: MealPlans/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create (MealPlan mealPlan)
    {
        if (ModelState.IsValid)
        {
            mealPlan.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            _context.MealPlans.Add(mealPlan);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(mealPlan);
    }

    // GET: MealPlans/Edit
    public async  Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mealPlan = await _context.MealPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && mp.UserId == userId);

        if (mealPlan == null)
        {
            return NotFound();
        }

        return View(mealPlan);
    }

    // POST: MealPlans/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, MealPlan mealPlan)
    {
        if (id != mealPlan.Id)
        {
            return BadRequest();
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var existingPlan = await _context.MealPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && mp.UserId == userId);

        if (existingPlan == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            existingPlan.Title = mealPlan.Title;
            existingPlan.StartDate = mealPlan.StartDate;
            existingPlan.EndDate = mealPlan.EndDate;
            existingPlan.Status = mealPlan.Status;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(existingPlan);
    }

    // GET: MealPlans/Delete
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mealPlan = await _context.MealPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && mp.UserId == userId);

        if (mealPlan == null)
        {
            return NotFound();
        }

        return View(mealPlan);
    }

    // POST: MealPlans/Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed (int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mealPlan = await _context.MealPlans
            .FirstOrDefaultAsync(mp => mp.Id == id && mp.UserId == userId);

        if (mealPlan == null)
        {
            return NotFound();
        }

        _context.MealPlans.Remove(mealPlan);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // GET: MealPlans/AddMeal
    public async  Task<IActionResult> AddMeal(int id)
    {
        var mealPlan = await _context.MealPlans
            .Include(mp => mp.Meals)
            .FirstOrDefaultAsync(mp => mp.Id == id);

        if (mealPlan == null)
            return NotFound();

        ViewBag.AllMeals = await _context.Meals.ToListAsync();
        return View(mealPlan);
    }

    [HttpPost]
    public async Task<IActionResult> AddMeal (int mealPlanId, int mealId)
    {
        var mealPlan = await _context.MealPlans
            .Include(mp => mp.Meals)
            .FirstOrDefaultAsync(mp => mp.Id  == mealPlanId);

        var meal = await _context.Meals.FindAsync(mealId);

        if (mealPlan == null || meal == null)
            return NotFound();

        if (!mealPlan.Meals.Contains(meal))
        {
            mealPlan.Meals.Add(meal);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

}
