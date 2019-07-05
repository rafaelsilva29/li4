using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookieFy.Models
{
    public class Recipe
    {
        //Props
        public int RecipeID { get; set; } // to identify a recipe in db

        [Required] // model validation
        [StringLength(30)]
        public string Title { get; set; } // recipe title

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // recipe description

        [Required]
        public int Rank { get; set; } = 0;

        [Required]
        public int Classifications { get; set; } = 0;

        [Required]
        public int Time { get; set; } // time to do the recipe

        [Required]
        public double Price { get; set; } = 0.0; // time to do the recipe

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? Date { get; set; } // creation date

        public byte[] ImgRecipe { get; set; } = new byte[1024]; //  user profile pic path

        // ------ Relationships ------ //
        // Recipes
        public int UserID { get; set; }
        public User User { get; set; }
        // Comments
        public List<Comment> Comments { get; set; }
        // WeekPlanRecipes
        public List<RecipePlan> RecipesPlan { get; set; }
        // RecipeUtensils
        public List<RecipeUtensils> RecipeUtensils { get; set; }
        // RecipeIngredients
        public List<RecipeIngredient> RecipeIngredients { get; set; }
        // RecipeTags
        public List<RecipeTag> RecipeTags { get; set; }
        // SubRecipes
        public List<Recipe> SubRecipes { get; set; }
        public Recipe SubRecipe { get; set; }
        // Steps
        public List<Step> Steps { get; set; }
    }
}