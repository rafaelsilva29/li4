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
    public class EditProfile : PageModel {

        [Required]
        [BindProperty]
        [Display(Name = "Nome")]
        public String NameModel { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Profissão")]
        public String ProfessionModel { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Pais")]
        public String CountryModel { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Cidade")]
        public String CityModel { get; set; }


        [Required]
        [BindProperty]
        [Display(Name = "Desccrição")]
        public String DescriptionModel { get; set; }

        [BindProperty]
        [Display(Name = "Avatar")]
        public IFormFile AvatarModel { get; set; }

        private readonly CookieDbContext dbContext;

        public EditProfile(CookieDbContext context) {

            dbContext = context;
        }

        // metodo que é chamado automaticamente quando se entra na página
        public void OnGet() {
        
            if (User.IsInRole("User")) {

                // vai buscar o utilizador à BD
                User user = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));

                // passa para variável local da página que será mostrada na view
                NameModel = user.Name;
                ProfessionModel = user.Profession;
                CountryModel = user.Country;
                CityModel = user.City;
                DescriptionModel = user.Description;
            }
        }

        // recebe a imagem que o user deu update
        public MemoryStream GetAvatar(IFormFile imageUploaded) {

            // define a path do avatar default
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user.png");

            byte[] data = null;

            MemoryStream ms1 = new MemoryStream();

            // se a imagem do upload é diferente de nula e se é realmente uma IMAGEM
            if (imageUploaded != null && imageUploaded.ContentType.ToLower().StartsWith("image/")) {

                MemoryStream ms = new MemoryStream();

                // passa a imagem para um array de bytes
                imageUploaded.OpenReadStream().CopyTo(ms);
                data = ms.ToArray();

                // usa uma biblioteca de tratamento de imagens
                using (MagickImage image = new MagickImage(data)) {

                    // define um tamanho para a imagem (150x150)
                    MagickGeometry size = new MagickGeometry(150, 150);
                    size.IgnoreAspectRatio = true;

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
            else {

                // vai buscar a imagem padrão
                using (MagickImage image = new MagickImage(path)) {

                    // define tamanho 150x150
                    MagickGeometry size = new MagickGeometry(150, 150);
                    size.IgnoreAspectRatio = true;

                    // dá resize à imagem
                    image.Resize(size);

                    // guarda o resultado
                    image.Write(ms1);

                }
            }

            return ms1;
        }

        // método chamado quando se carrega no botão editar user
        [Authorize(Roles = "User")]
        public IActionResult OnPostUpdateUserAccount() {

            TryUpdateModelAsync(this);

            //ModelState.Remove("AvatarModel");

            // vai buscar o user à BD
            User oldUser = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));

            // retorna a página em si pq os campos estão mal preenchidos
            if (!ModelState.IsValid) return Page();

            // calcula o avatar/imagem
            if (AvatarModel != null) {

                oldUser.ImgPath = GetAvatar(AvatarModel).ToArray();
            }

            // muda os dados do utilizador na BD
            oldUser.Name = NameModel;
            oldUser.Profession = ProfessionModel;
            oldUser.Country = CountryModel;
            oldUser.City = CityModel;
            oldUser.Description = DescriptionModel;

            dbContext.Entry(oldUser).State = EntityState.Modified;

            // guarda as alterações
            dbContext.SaveChanges();

            return RedirectToPage("/UserArea", "UserLoggedIn").WithSuccess("Perfil", "editado com sucesso.", "3000");

        }

    }
}