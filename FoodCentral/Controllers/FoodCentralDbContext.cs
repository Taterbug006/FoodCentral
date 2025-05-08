using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodCentral.Models;

namespace FoodCentral
{
    public class FoodCentralDbContext : IdentityDbContext
    {
        public FoodCentralDbContext(DbContextOptions<FoodCentralDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    Id = 1,
                    Name = "Spaghetti",
                    MealType = "Dinner",
                    IsVegetarian = false,
                    Instructions = "Boil noodles. Add sauce. Serve hot."
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Tacos",
                    MealType = "Lunch",
                    IsVegetarian = false,
                    Instructions = "Cook beef. Place in shells with cheese and lettuce."
                },
                new Recipe
                {
                    Id = 3,
                    Name = "Hamburgers",
                    MealType = "Dinner",
                    IsVegetarian = false,
                    Instructions = "Grill patties. Place in buns with toppings."
                },
                new Recipe
                {
                    Id = 4,
                    Name = "Fried Fish",
                    MealType = "Lunch",
                    IsVegetarian = false,
                    Instructions = "Coat fish in batter. Fry until golden. Serve with lemon."
                }
            );
        }
    }
}
