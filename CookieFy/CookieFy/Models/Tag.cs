using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class Tag
    {
        //Prop
        public int TagID { get; set; }

        [Required]
        public string Name { get; set; }

        // ------ Relationships ------ //
        public List<RecipeTag> RecipeTags { get; set; }
    }
}

