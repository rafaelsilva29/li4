using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Diagnostics;
using ImageMagick;
using CookieFy.Data;
using CookieFy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CookieFy.Includes;
using Microsoft.EntityFrameworkCore;

namespace CookieFy.Pages
{
    [AllowAnonymous]
    public class RegisterSubscription : PageModel
    {
        private readonly CookieDbContext dbContext;

        [BindProperty]
        public Subscription Subscription { get; set; }

        public RegisterSubscription(CookieDbContext context)
        {

            dbContext = context;
        }

        // metodo acedido quando o utilizador carrega em criar conta
        [AllowAnonymous]
        public IActionResult OnPostCreateSubscription()
        {

            TryUpdateModelAsync(this);

            //ModelState.Remove("User.Avatar");

            // retorna erros se os campos foram incorretamente preenchidos ou não preenchidos
            if (!ModelState.IsValid) return Page();

            if (ModelState.IsValid)
            {
                // verifica se o email já existe
                bool subAlreadyExists = dbContext.Subscription.Any(s => s.Name == Subscription.Name);

                // retorna erro em caso de já existir
                if (subAlreadyExists)
                {
                    ModelState.AddModelError(string.Empty, "Esta subscrição ja existe no sistema.");
                    return Page();
                }

   
            }

            // cria a subscrição
            Subscription newSub = new Subscription()
            {
                Name = Subscription.Name,
                Price = Subscription.Price
            };

            // adiciona cliente à BD
            dbContext.Subscription.Add(newSub);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("./AdminArea").WithSuccess("Subscrição", "registado com sucesso.", "3000");

        }
    }
}