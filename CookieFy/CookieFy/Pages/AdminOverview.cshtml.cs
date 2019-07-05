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

    [Authorize(Roles = "Admin")]
    public class AdminOverview : PageModel {

        public List<User> UsersList { get; set; }

        public List<Subscription> SubscriptionList { get; set; }

        public List<Recipe> RecipesList { get; set; }

        private readonly CookieDbContext dbContext;

        public AdminOverview(CookieDbContext context) {

            dbContext = context;
        }

        public IActionResult OnGet() {

            return Unauthorized();
        }
            
        // metodo chamado quando se carrega no botão lista de utilizadores
        public ActionResult OnGetListOfUsers() {

            // traz os clientes da BD, ordenados pelo name
            var query = (from p in dbContext.User
                         orderby p.Name
                         select p).ToList();

            UsersList = new List<User>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query) {

                // coloca numa lista que será lida pela view
                UsersList.Add(new User() {
                    UserID = item.UserID,
                    Name = item.Name,
                    Email = item.Email,
                    Profession = item.Profession,
                    Country = item.Country,
                    City = item.City,
                    Balance = item.Balance,
                    Description = item.Description,
                    ImgPath = item.ImgPath,
                });
            }

            return Page();
        }

        // metodo chamado quando se carrega no botão lista de receitas
        public ActionResult OnGetListOfRecipes() {

            // traz as receitas da BD ordendas por rank
            var query = (from p in dbContext.Recipe
                         orderby p.Rank
                         select p).ToList();

            RecipesList = new List<Recipe>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query) {

                // coloca numa lista que será lida pela view
                RecipesList.Add(new Recipe() {
                    RecipeID = item.RecipeID,
                    Title = item.Title,
                    Description = item.Description,
                    Rank = item.Rank,
                    Classifications = item.Classifications,
                    Time = item.Time,
                    Date = item.Date,
                    Price = item.Price,
                });
            }

            return Page();
        }

        // metodo chamado quando se carrega no botão lista de subcrições
        public ActionResult OnGetListOfSubscriptions() {

            // traz da BD as subscrições
            var query = (from p in dbContext.Subscription
                         orderby p.Name
                         select p).ToList();

            SubscriptionList = new List<Subscription>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query) {

                // coloca numa lista que será lida pela view
                SubscriptionList.Add(new Subscription() {
                    SubscriptionID = item.SubscriptionID,
                    Name = item.Name,
                    Price = item.Price
                });
            }

            return Page();
        }

        // metodo chamado quando se carrega no botão apagar user
        public IActionResult OnPostDeleteUser(int userid) {

            // vai buscar o utilizador à BD
            User user = dbContext.User.Find(userid);

            // remove o utilizador
            dbContext.User.Remove(user);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/AdminOverview", "ListOfUsers").WithSuccess("Admin", "O utilizador foi removido com sucesso.", "2000");
            
        }

        // metodo chamado quando se carrega no botão apagar recipe
        public IActionResult OnPostDeleteRecipe(int recipeid) {

            // vai buscar a receita à BD
            Recipe rec = dbContext.Recipe.Find(recipeid);

            // remove o cliente
            dbContext.Recipe.Remove(rec);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/AdminOverview", "ListOfRecipes").WithSuccess("Admin", "A receita foi removida com sucesso.", "2000");
        }

        // metodo chamado quando se carrega no botão apagar recipe
        public IActionResult OnPostDeleteSubscription(int subid)
        {
            // vai buscar a subscricao a BD
            Subscription sub = dbContext.Subscription.Find(subid);

            // remove a subscriçao
            dbContext.Subscription.Remove(sub);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/AdminOverview", "ListOfSubscriptions").WithSuccess("Admin", "A subscrição foi removido com sucesso.", "2000");
        }
    }
}