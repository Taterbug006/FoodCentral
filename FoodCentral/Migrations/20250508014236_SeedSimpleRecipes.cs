using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodCentral.Migrations
{
    /// <inheritdoc />
    public partial class SeedSimpleRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Instructions", "IsVegetarian", "MealType", "Name" },
                values: new object[,]
                {
                    { 1, "Boil noodles. Add sauce. Serve hot.", false, "Dinner", "Spaghetti" },
                    { 2, "Cook beef. Place in shells with cheese and lettuce.", false, "Lunch", "Tacos" },
                    { 3, "Grill patties. Place in buns with toppings.", false, "Dinner", "Hamburgers" },
                    { 4, "Coat fish in batter. Fry until golden. Serve with lemon.", false, "Lunch", "Fried Fish" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
