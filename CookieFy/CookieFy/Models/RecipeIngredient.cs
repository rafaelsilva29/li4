using System;
namespace CookieFy.Models
{
    public class RecipeIngredient
    {
        public int RecipeIngredientID { get; set; }

        // ------ Relationships ------ //
        public int? RecipeID { get; internal set; }
        public int? IngredientID { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public Ingredient Ingredient { get; internal set; }
    }
}