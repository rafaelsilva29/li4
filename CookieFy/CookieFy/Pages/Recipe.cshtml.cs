using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CookieFy.Data;
using CookieFy.Models;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Authentication;
using CookieFy.Includes;

namespace CookieFy.Pages
{
    [Authorize(Roles = "User")]
    public class RecipeModel : PageModel
    {
        [BindProperty]
        public Recipe TRecipe { get; set; }

        [BindProperty]
        public List<Ingredient> RIngredients { get; set; } = new List<Ingredient>();

        [BindProperty]
        public List<Utensil> RUtensils { get; set; } = new List<Utensil>();

        [BindProperty]
        public List<Comment> RComments { get; set; } = new List<Comment>();

        private readonly CookieDbContext dbContext;

        public RecipeModel(CookieDbContext context)
        {
            dbContext = context;
        }

        public IActionResult OnGet()
        {
            return Unauthorized();
        }

       
        public IActionResult OnGetRecipe(int recipeID)
        {
            TRecipe = dbContext.Recipe.Find(recipeID);

            var Ingredients = dbContext.RecipeIngredient.Where(r => r.RecipeID == recipeID);

            foreach(var item in Ingredients)
            {
                RIngredients.Add(dbContext.Ingredient.Where(r => r.IngredientID == item.IngredientID).FirstOrDefault());
            }

            var Utensils = dbContext.RecipeUtensils.Where(r => r.RecipeID == recipeID);

            foreach (var item in Utensils)
            {
                RUtensils.Add(dbContext.Utensil.Where(r => r.UtensilID == item.UtensilID).FirstOrDefault());
            }

            RComments = dbContext.Comment.Where(r => r.RecipeID == recipeID).ToList();

            if (TRecipe != null) 
            {
                return Page();
            }

            return RedirectToPage("/AllRecipes", "ListOfRecipes").WithDanger("Error", recipeID + "Sem dados suficientes", "5000");
        }
    }
}