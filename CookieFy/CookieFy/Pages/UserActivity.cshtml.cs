using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookieFy.Models;
using CookieFy.Data;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using CookieFy.Includes;
using Microsoft.AspNetCore.Authorization;

namespace CookieFy.Pages {

    [Authorize(Roles = "User")]
    public class UserActivity : PageModel {

        private readonly CookieDbContext dbContext;

        public List<Recipe> Recipes { get; set; }

        public List<RecipeList> RecipeList { get; set; }

        public List<RecipePlan> RecipePlan { get; set; }

        public UserActivity(CookieDbContext context) {

            dbContext = context;
        }

        public IActionResult OnGet() {

            return Unauthorized();
        }

        // método que é chamado quando se carrega no histórico de trabalhos
        public ActionResult OnGetHistoryList() {



            return Page();
        }

        // método que é chamado quando se carrega nos trabalhos pendentes
        public ActionResult OnGetPendentList() {

       
            return Page();
        }

        // método que é chamado quando se carrega nos trabalhos oferecidos
        public ActionResult OnGetOffersList() {


            return Page();
        }

        // método chamado pelo botão cancelar oferta. Traz o id do trabalho em questão...
        public IActionResult OnPostCancelOffer(int id) {

            return RedirectToPage("/UserArea", "ClientLoggedIn").WithSuccess("Oferta", "cancelada com sucesso.", "2000");

        }

        // método chamado pelo botão avaliar trabalho. Traz o id do trabalho em questão...
        public IActionResult OnPostRateOffer(int id) {



            return RedirectToPage("/UserArea", "ClientLoggedIn").WithSuccess("Trabalho", "avaliado com sucesso.", "2000");
        }


    }
}