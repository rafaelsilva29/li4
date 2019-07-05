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
    public class DoRecipeModel : PageModel
    {
        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public List<Step> RSteps { get; set; } = new List<Step>();

        private readonly CookieDbContext dbContext;

        public DoRecipeModel(CookieDbContext context)
        {
            dbContext = context;
        }

        public IActionResult OnGet()
        {
            return Unauthorized();
        }

        public IActionResult OnGetReady(int recipeID)
        {
            Recipe = dbContext.Recipe.Find(recipeID);
            RSteps = dbContext.Step.Where(s => s.RecipeID == recipeID).OrderBy(s => s.Position).ToList();

            if (RSteps.Count == 0)
            {
                return RedirectToPage("/Assistent", "Recipe", recipeID).WithWarning("Warning", "Receita sem passos adicionados!", "5000");
            }

            if (RSteps != null && Recipe != null)
            {
                return Page();
            }

            return RedirectToPage("/Assistent", "Recipe", recipeID).WithDanger("Error", "Receita indisponível para confeção!", "5000");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/AssistentFinalPage", "Recipe", new { recipeid = Recipe.RecipeID }).WithSuccess(" Sucesso ", " Concluiu uma receita! ", "2000");
        }
    }
}
