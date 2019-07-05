using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using CookieFy.Data;
using CookieFy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CookieFy.Includes;


namespace CookieFy.Pages
{
    [AllowAnonymous]
    public class AssistentFinalPageModel : PageModel
    {
        [BindProperty]
        public int recipeid { get; set; }

        [BindProperty]
        public int Rank { get; set; }

        [BindProperty]
        public Comment Comment { get; set; }

        private readonly CookieDbContext dbContext;

        public AssistentFinalPageModel(CookieDbContext context)
        {

            dbContext = context;
        }

        public void OnGetRecipe(int recipeid) {
        
        }

        public IActionResult OnGet()
        {

            return Unauthorized();
        }

        // metodo acedido quando o utilizador pretende classificar uma receita finalizada
        [Authorize(Roles = "User")]
        public IActionResult OnPostAddClassification()
        {

            TryUpdateModelAsync(this);


            Comment newComment = new Comment()
            {
                Text = Comment.Text,
                RecipeID = recipeid
            };
            //adiciona o novo comentario à BD
            dbContext.Comment.Add(newComment);


            Recipe oldRecipe = dbContext.Recipe.Find(recipeid);

            if (!ModelState.IsValid) return Page();



            int novoRank = (oldRecipe.Rank * oldRecipe.Classifications + Rank) / (oldRecipe.Classifications + 1);

            oldRecipe.Rank = novoRank;

            oldRecipe.Classifications++;

            dbContext.Entry(oldRecipe).State = EntityState.Modified;





            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/UserArea", "ClientLoggedIn").WithSuccess("Trabalho", "avaliado com sucesso.", "2000");
        }
    }
}
