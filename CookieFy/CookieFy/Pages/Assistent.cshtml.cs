using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookieFy.Data;
using CookieFy.Models;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Authentication;
using CookieFy.Includes;

namespace CookieFy.Pages
{
    [Authorize(Roles = "User")]
    public class AssistentModel : PageModel
    {
        [BindProperty]
        public Recipe Recipe { get; set; }

        private readonly CookieDbContext dbContext;

        public AssistentModel(CookieDbContext context)
        {
            dbContext = context;
        }

        public IActionResult OnGet()
        {
            return Unauthorized();
        }

        public IActionResult OnGetRecipe(int recipeID)
        {
            var recipe = dbContext.Recipe.Include(r => r.RecipeIngredients)
                                            .ThenInclude(ingredient => ingredient.Ingredient)
                                         .Include(r => r.RecipeUtensils)
                                            .ThenInclude(utensil => utensil.Utensil)
                                         .FirstOrDefault(r => r.RecipeID == recipeID);

            Recipe = recipe;

            if (Recipe != null && Recipe.RecipeIngredients != null && Recipe.RecipeUtensils != null)
            {
                return Page();
            }

            return RedirectToPage("/Recipe", "Recipe", recipeID).WithDanger("Error", "Receita indisponível para confeção!", "5000");
        }
    }
}
