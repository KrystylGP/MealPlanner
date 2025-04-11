using MealPlanner.Data;
using MealPlanner.Data.Entities;
using MealPlanner.Models.ViewModel;
using MealPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers;

[Authorize]
public class IngredientsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IngredientService _ingredientService;

    public IngredientsController(ApplicationDbContext context, IngredientService ingredientService)
    {
        _context = context;
        _ingredientService = ingredientService;
    }

    // Visar ingredienser
    public async Task<IActionResult> Index()
    {
        var ingredients = await _ingredientService.GetAllAsync();
        return View(ingredients);
    }


    // Visar MyIngredients (användarens ingredienser hemma)
    public async Task<IActionResult> MyIngredients()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var userIngredients = await _ingredientService.GetUserIngredientsAsync(userId);
        return View(userIngredients);
    }


    // GET: Ingredients/AddUserIngredient
    public async Task<IActionResult> AddUserIngredient()
    {
        // Fetch available ingredients from the service
        var availableIngredients = await _ingredientService.GetAllAsync();

        var model = new AddUserIngredientViewModel
        {
            AvailableIngredients = availableIngredients
        };

        return View(model);
    }

    // POST: Ingredients/AddUserIngredient
    [HttpPost]
    public async Task<IActionResult> AddUserIngredient(AddUserIngredientViewModel model)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null || !ModelState.IsValid)
        {
            model.AvailableIngredients = await _ingredientService.GetAllAsync();
            return View(model);
        }

        await _ingredientService.AddUserIngredientAsync(userId, model.IngredientId, model.Quantity);

        return RedirectToAction(nameof(MyIngredients));
    }

    // POST: Ingredients/DeleteMyIngredient
    [HttpPost]
    public async Task<IActionResult> DeleteMyIngredient(int ingredientId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var result = await _ingredientService.DeleteUserIngredientAsync(userId, ingredientId);

        if (result)
            return RedirectToAction(nameof(MyIngredients));

        return NotFound();
    }

    public async Task<IActionResult> GroceryList()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        // Calling the service to generate the grocery list
        var groceryList = await _ingredientService.GenerateGroceryListAsync(userId);

        return View("GroceryList", groceryList);
    }


    // GET: Ingredients/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Ingredients/Create
    [HttpPost]
    public async Task<IActionResult> Create(Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            var result = await _ingredientService.AddIngredientAsync(ingredient);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(ingredient);
    }

    // GET: Ingredients/Edit
    public async Task<IActionResult> Edit(int id)
    {
        var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredient == null)
            return NotFound();

        return View(ingredient);
    }

    // POST: Ingredients/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Ingredient ingredient)
    {
        if (id != ingredient.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            var result = await _ingredientService.UpdateIngredientAsync(ingredient);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        return View(ingredient);
    }

    // GET: Ingredients/EditUserIngredient
    public async Task<IActionResult> EditUserIngredient(int ingredientId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        // Use a new service method name to fetch the user's ingredient
        var userIngredient = await _ingredientService.FetchUserIngredientAsync(userId, ingredientId);
        if (userIngredient == null)
            return NotFound();

        var model = new AddUserIngredientViewModel
        {
            IngredientId = userIngredient.IngredientId,
            Quantity = userIngredient.Quantity,
            AvailableIngredients = new List<Ingredient> { userIngredient.Ingredient }
        };

        return View(model);
    }

    // POST: Ingredients/EditUserIngredient
    [HttpPost]
    public async Task<IActionResult> EditUserIngredient(AddUserIngredientViewModel model)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !ModelState.IsValid)
        {
            model.AvailableIngredients = await _ingredientService.GetAllAsync();
            return View(model);
        }

        // Use a new service method name for updating the quantity
        var success = await _ingredientService.UpdateUserIngredientQuantityAsync(userId, model.IngredientId, model.Quantity);
        if (!success)
            return NotFound();

        return RedirectToAction(nameof(MyIngredients));
    }

}
