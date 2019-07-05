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

namespace CookieFy.Pages {

    [Authorize(Roles = "User")]
    public class UserArea : PageModel {

        [BindProperty]
        public User user { get; set; }

        private readonly CookieDbContext dbContext;

        public UserArea(CookieDbContext context)
        {
            dbContext = context;
        }

        // método acedido automaticamente qd se entra na página
        public ActionResult OnGet()
        {
            // retorna unauthorized se o utilizador atual não estiver logado
            if (user == null) return Unauthorized();
            return Page();
        }

        // retorna a área do cliente
        public ActionResult OnGetUserLoggedIn()
        {
            // email do utilizador logado
            var email = User.Identity.Name;
            // vai buscar o cliente à BD
            user = dbContext.User.FirstOrDefault(u => u.Email.Equals(email));
            return Page();
        }
    }
}