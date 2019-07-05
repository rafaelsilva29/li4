using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class Utensil
    {
        //Prop
        public int UtensilID { get; set; }

        [Required]
        public string Name { get; set; }


        // ------ Relationships ------ //
        // RecipeUtensils
        public List<RecipeUtensils> RecipeUtensils { get; set; }
    }
}
