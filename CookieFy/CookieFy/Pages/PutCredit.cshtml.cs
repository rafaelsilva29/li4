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
using CookieFy.Models;
using CookieFy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CookieFy.Includes;
using Microsoft.EntityFrameworkCore;

namespace CookieFy.Pages {

    [Authorize(Roles = "User")]
    public class PutCredit : PageModel {

        [Required]
        [BindProperty]
        [Display(Name = "Número Cartão de Crédito")]
        public int NumberCC { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "CVC")]
        public int CVC { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Validade do Cartão de Crédito")]
        public string Date { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Montante")]
        public double mount { get; set; }

        private readonly CookieDbContext dbContext;

        public PutCredit(CookieDbContext context) {

            dbContext = context;
        }

        public IActionResult OnPostPutCredit() {

            TryUpdateModelAsync(this);

            User oldUser = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));

            // muda os dados do profissional na BD
            oldUser.Balance += mount;

            dbContext.Entry(oldUser).State = EntityState.Modified;

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/UserArea", "UserLoggedIn").WithSuccess("Carregamento", "efectuado com sucesso.", "3000");

        }

        public IActionResult OnGetCancel()
        {
            return RedirectToPage("/UserArea", "UserLoggedIn").WithDanger("Carregamento", "cancelado.", "3000");
        }
    }
}