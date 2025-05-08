namespace FoodCentral.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MealType { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public string? UserId { get; set; }
    }
}
