using MealPlanner.Data.Entities;
using MealPlanner.Models.Enums;

namespace MealPlanner.Data.MockData;

public static class DataSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Ingredienser
        var kyckling = context.Ingredients.FirstOrDefault(i => i.Name == "Kyckling");
        if (kyckling == null)
        {
            kyckling = new Ingredient { Name = "Kyckling", Type = IngredientType.Meat };
            context.Ingredients.Add(kyckling);
            context.SaveChanges();
        }

        var curry = context.Ingredients.FirstOrDefault(i => i.Name == "Curry");
        if (curry == null)
        {
            curry = new Ingredient { Name = "Curry", Type = IngredientType.Spice };
            context.Ingredients.Add(curry);
            context.SaveChanges();
        }

        // Måltider
        var kycklinggryta = context.Meals.FirstOrDefault(m => m.Name == "Kycklinggryta");
        if (kycklinggryta == null)
        {
            kycklinggryta = new Meal
            {
                Name = "Kycklinggryta",
                CookingTime = 45,
                MealIngredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = kyckling, Quantity = 1 },
                    new MealIngredient { Ingredient = curry, Quantity = 1 }
                }
            };
            context.Meals.Add(kycklinggryta);
        }

        var grilladKyckling = context.Meals.FirstOrDefault(m => m.Name == "Grillad Kyckling");
        if (grilladKyckling == null)
        {
            grilladKyckling = new Meal
            {
                Name = "Grillad Kyckling",
                CookingTime = 35,
                MealIngredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = kyckling, Quantity = 1 }
                }
            };
            context.Meals.Add(grilladKyckling);
        }

        context.SaveChanges();

        // MealPlan
        var userId = "6ef43d07-47b3-498c-858f-fa8fc70a2b41";
        if (!context.MealPlans.Any(mp => mp.Title == "Kycklingvecka"))
        {
            var plan = new MealPlan
            {
                Title = "Kycklingvecka",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(6),
                Status = MealPlanStatus.InProgress,
                UserId = userId,
                Meals = new List<Meal> { kycklinggryta, grilladKyckling }
            };

            context.MealPlans.Add(plan);
            context.SaveChanges();
        }

        // Användarens ingredienser
        if (!context.UserIngredients.Any(ui => ui.UserId == userId && ui.IngredientId == kyckling.Id))
        {
            context.UserIngredients.Add(new UserIngredient
            {
                UserId = userId,
                IngredientId = kyckling.Id,
                Quantity = 1
            });
            context.SaveChanges();
        }
    }
}
