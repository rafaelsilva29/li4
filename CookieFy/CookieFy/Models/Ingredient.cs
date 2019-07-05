using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class Ingredient
    {
        //Prop
        public int IngredientID { get; set; }

        [Required]
        public string Name { get; set; }

        // ------ Relationships ------ //
        public List<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
