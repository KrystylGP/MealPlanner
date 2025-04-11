using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MealPlanner.Data.Entities;
using MealPlanner.Models.VM;  
using MealPlanner.Services;
using System.Threading.Tasks;

namespace MealPlanner.Controllers
{
    [Authorize]
    public class MealsController : Controller
    {
        private readonly MealService _mealService;

        public MealsController(MealService mealService)
        {
            _mealService = mealService;
        }

        // GET: Meals/Index
        public async Task<IActionResult> Index()
        {
            var meals = await _mealService.GetAllMealsAsync();
            return View(meals);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            // Using a MealCreateViewModel that contains a list of available ingredients
            var viewModel = new MealCreateViewModel
            {
                Ingredients = _mealService.GetAllIngredientsForMealCreate()
            };

            return View(viewModel);
        }

        // POST: Meals/Create
        [HttpPost]
        public async Task<IActionResult> Create(MealCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Ingredients = _mealService.GetAllIngredientsForMealCreate();
                return View(model);
            }


            var meal = new Meal
            {
                Name = model.Name,
                CookingTime = model.CookingTime,
                MealIngredients = model.Ingredients
                    .Where(i => i.Selected && i.Quantity > 0)
                    .Select(i => new MealIngredient
                    {
                        IngredientId = i.IngredientId,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };

            await _mealService.AddMealAsync(meal);
            return RedirectToAction(nameof(Index));
        }

        // GET: Meals/Edit
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
                var result = await _mealService.UpdateMealAsync(meal);
                if (!result)
                    return NotFound();

                return RedirectToAction(nameof(Index));
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

            var result = await _mealService.DeleteMealAsync(meal);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: Meals/Details
        public async Task<IActionResult> MealDetails(int id)
        {
            var meal = await _mealService.GetMealWithIngredientsByIdAsync(id);
            if (meal == null)
                return NotFound();

            return View("MealDetails", meal);
        }
    }
}
