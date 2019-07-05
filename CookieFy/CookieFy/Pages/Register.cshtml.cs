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
    public class Register : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Confirmar Password")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public IFormFile Avatar { get; set; }

        private readonly CookieDbContext dbContext;

        [BindProperty]
        public User User { get; set; }

        public Register(CookieDbContext context)
        {

            dbContext = context;
        }

        // recebe a imagem que o user deu update
        public MemoryStream GetAvatar(IFormFile imageUploaded)
        {

            // define a path do avatar default
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user.png");

            byte[] data = null;

            MemoryStream ms1 = new MemoryStream();

            // se a imagem do upload é diferente de nula e se é realmente uma IMAGEM
            if (imageUploaded != null && imageUploaded.ContentType.ToLower().StartsWith("image/", StringComparison.Ordinal))
            {

                MemoryStream ms = new MemoryStream();

                // passa a imagem para um array de bytes
                imageUploaded.OpenReadStream().CopyTo(ms);
                data = ms.ToArray();

                // usa uma biblioteca de tratamento de imagens
                using (MagickImage image = new MagickImage(data))
                {

                    // define um tamanho para a imagem (150x150)
                    MagickGeometry size = new MagickGeometry(150, 150)
                    {
                        IgnoreAspectRatio = true
                    };

                    // dá resize da imagem
                    image.Resize(size);

                    // converte-a para JPEG
                    image.Format = MagickFormat.Jpeg;

                    // guarda o resultado
                    image.Write(ms1);

                }

                return ms1;
            }

            // se o que deu upload não é uma imagem ou se é null
            else
            {

                // vai buscar a imagem padrão
                using (MagickImage image = new MagickImage(path))
                {

                    // define tamanho 150x150
                    MagickGeometry size = new MagickGeometry(150, 150)
                    {
                        IgnoreAspectRatio = true
                    };

                    // dá resize à imagem
                    image.Resize(size);

                    // guarda o resultado
                    image.Write(ms1);

                }
            }

            return ms1;
        }

        // metodo acedido quando o utilizador carrega em criar conta
        [AllowAnonymous]
        public IActionResult OnPostCreateAccount()
        {

            TryUpdateModelAsync(this);

            //ModelState.Remove("User.Avatar");

            // retorna erros se os campos foram incorretamente preenchidos ou não preenchidos
            if (!ModelState.IsValid) return Page();

            if (ModelState.IsValid)
            {
                // verifica se o email já existe
                bool userEmailAlreadyExists = dbContext.User.Any(u => u.Email == User.Email);

                // retorna erro em caso de já existir
                if (userEmailAlreadyExists || User.Email.Equals("admin"))
                {
                    ModelState.AddModelError(string.Empty, "Este email já existe no sistema.");
                    return Page();
                }

                // retorna erro se as passwords não coincidirem
                if (ConfirmPassword != User.Password) 
                {
                    ModelState.AddModelError(string.Empty, "Passwords não coincidem");
                    return Page();
                }
            }

            // encripta a password
            var hash = BCrypt.Net.BCrypt.HashPassword(User.Password);

            // cria o cliente
            User newUser = new User()
            {
                Name = User.Name,
                Profession = User.Profession,
                Country = User.Country,
                City = User.City,
                Balance = 0.0,
                Email = User.Email,
                ImgPath = GetAvatar(Avatar).ToArray(),
                Description = User.Description,
                Password = hash
            };

            // adiciona cliente à BD
            dbContext.User.Add(newUser);

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("./Index").WithSuccess("Utilizador", "registado com sucesso.", "3000");

        }
    }
}