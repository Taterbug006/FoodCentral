using System.ComponentModel.DataAnnotations;

namespace FoodCentral.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RecipeId { get; set; }
    }

}
