using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class AllRecipesModel : PageModel
    {
        public List<Recipe> RecipesList { get; set; }

        private readonly CookieDbContext dbContext;

        public AllRecipesModel(CookieDbContext context)
        {

            dbContext = context;
        }

        public IActionResult OnGet()
        {
            return Unauthorized();
        }

        // metodo chamado quando se carrega no botão lista de receitas
        public ActionResult OnGetListOfRecipes()
        {

            // traz as receitas da BD ordendas por rank
            var query = (from p in dbContext.Recipe
                         orderby p.Rank
                         select p).ToList();

            RecipesList = new List<Recipe>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query)
            {
                // coloca numa lista que será lida pela view
                RecipesList.Add(new Recipe()
                {
                    RecipeID = item.RecipeID,
                    Title = item.Title,
                    Description = item.Description,
                    Rank = item.Rank,
                    Classifications = item.Classifications,
                    Price = item.Price,
                    Time = item.Time,
                    Date = item.Date,
                    ImgRecipe = item.ImgRecipe
                });
            }

            return Page();
        }
    }
}
