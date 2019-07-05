using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class RecipeList
    {
        public int RecipeListID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Rank { get; set; }

        [Required]
        public int Classifications { get; set; }

        // ------ Relationships ------ //
        public int UserID { get; set; }
        public User User { get; internal set; }

        public List<RecipeFromList> Recipes { get; set; }
    }
}

