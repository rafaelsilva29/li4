using System;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class Step
    {
        //Props
        public int StepID { get; set; }

        public string Link { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int Time { get; set; }

        // ------ Relationships ------ //
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}