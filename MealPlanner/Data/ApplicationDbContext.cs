using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data.Entities;

namespace MealPlanner.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Meal> Meals { get; set; } = null!;
    public DbSet<MealPlan> MealPlans { get; set; } = null!;
    public DbSet<MealIngredient> MealIngredients { get; set; } = null!;
    public DbSet<UserIngredient> UserIngredients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // MealIngredient
        modelBuilder.Entity<MealIngredient>()
            .HasKey(mi => new { mi.MealId, mi.IngredientId });

        modelBuilder.Entity<MealIngredient>()
            .HasOne(mi => mi.Meal)
            .WithMany(m => m.MealIngredients)
            .HasForeignKey(mi => mi.MealId);

        modelBuilder.Entity<MealIngredient>()
            .HasOne(mi => mi.Ingredient)
            .WithMany(i => i.MealIngredients)
            .HasForeignKey(mi => mi.IngredientId);

        // UserIngredient
        modelBuilder.Entity<UserIngredient>()
            .HasKey(ui => new { ui.UserId, ui.IngredientId });

        modelBuilder.Entity<UserIngredient>()
            .HasOne(ui => ui.User)
            .WithMany(u => u.UserIngredients)
            .HasForeignKey(ui => ui.UserId);

        modelBuilder.Entity<UserIngredient>()
            .HasOne(ui => ui.Ingredient)
            .WithMany(i => i.UserIngredients)
            .HasForeignKey(ui => ui.IngredientId);
    }
}