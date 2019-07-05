using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookieFy.Models
{
    public class RecipePlan
    {
        //Props
        public int RecipePlanID { get; set; }
        public DateTime Date { get; set; }

        // ------ Relationships ------ //
        public int? UserID { get; internal set; }
        public User User { get; internal set; }

        public int? RecipeID { get; internal set; }
        public Recipe Recipe { get; internal set; }

    }
}
