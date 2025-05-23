﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MealPlanner.Data.Entities;
using MealPlanner.Models.ViewModel;  
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
                model.Ingredients = await _mealService.GetIngredientSelectionsAsync();
                return View(model);
            }

            var success = await _mealService.CreateMealAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte skapa måltiden.");
                model.Ingredients = await _mealService.GetIngredientSelectionsAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Meals/Edit (OBS, ej tillämpat i frontend pga olämplig för base user)
        public async Task<IActionResult> Edit(int id)
        {
            var meal = await _mealService.GetMealByIdAsync(id);
            if (meal == null)
                return NotFound();

            return View(meal);
        }

        // POST: Meals/Edit (OBS, ej tillämpat i frontend pga olämplig för base user)
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

        // GET: Meals/Delete (OBS, ej tillämpat i frontend pga olämplig för base user)
        public async Task<IActionResult> Delete(int id)
        {
            var meal = await _mealService.GetMealByIdAsync(id);
            if (meal == null)
                return NotFound();

            return View(meal);
        }

        // POST: Meals/Delete (OBS, ej tillämpat i frontend pga olämplig för base user)
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
