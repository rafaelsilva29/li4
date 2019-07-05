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
    public class AddSubscriptionUser : PageModel {

        public List<Subscription> SubscriptionList { get; set; }

        public List<Subscription> UserSubscriptionList { get; set; }

        public string email;

        private readonly CookieDbContext dbContext;

        public AddSubscriptionUser(CookieDbContext context) {

            dbContext = context;
        }


        // metodo chamado quando se carrega no botão lista de subcrições
        public ActionResult OnGetListOfSubscriptions() {

            // traz da BD as subscrições
            var query1 = (from p in dbContext.Subscription
                         orderby p.Name
                         select p).ToList();

            SubscriptionList = new List<Subscription>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query1) 
            {
                // coloca numa lista que será lida pela view
                SubscriptionList.Add(new Subscription() {
                    SubscriptionID = item.SubscriptionID,
                    Name = item.Name,
                    Price = item.Price
                });
            }

            User userAux = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));
            int id = userAux.UserID;

            var query2 = (from sub in dbContext.Subscription
                          join us in dbContext.UserSubscription on sub.SubscriptionID equals us.SubscriptionID
                          join user in dbContext.User on us.UserID equals id
                          orderby sub.SubscriptionID
                          select new { sub, us, user }).ToList();

            UserSubscriptionList = new List<Subscription>();

            // vai buscar o conteúdo de toda a query
            foreach (var item in query2)
            {
                // coloca numa lista que será lida pela view
                UserSubscriptionList.Add(new Subscription()
                {
                    SubscriptionID = item.sub.SubscriptionID,
                    Name = item.sub.Name,
                    Price = item.sub.Price
                });
            }

            return Page();
        }

        // metodo chamado quando se carrega no botão de adicionar subscrição
        public IActionResult OnPostAddSubscription(int subid)
        {   
            //if(UserSubscriptionList.Count > 1) {
                User userAux = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));
                int id = userAux.UserID;
               
                UserSubscription us = new UserSubscription()
                {
                    SubscriptionID = subid,
                    UserID = id
                };

                dbContext.UserSubscription.Add(us);
                dbContext.SaveChanges();

                //return RedirectToPage("/AddSubscriptionUser", "ListOfSubscriptions").WithDanger("Subscrição", "já possui uma subcrição.", "2000");
           //}

            return RedirectToPage("/UserArea", "ClientLoggedIn").WithSuccess("Subscrição", "foi adicionada com sucesso.", "2000");
        }

        // metodo chamado quando se carrega no botão apagar subscrição
        public IActionResult OnPostCancelSubscription(int subid)
        {
            // vai buscar a subscricao a BD
            Subscription sub = dbContext.Subscription.Find(subid);

            // remove a subscriçao
            dbContext.Subscription.Remove(sub);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/UserArea", "ClientLoggedIn").WithSuccess("Subscrição", "adicionada com sucesso.", "2000");
        }
    }
}