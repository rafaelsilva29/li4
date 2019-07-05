using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using CookieFy.Data;
using CookieFy.Models;
using Microsoft.AspNetCore.Authorization;
using CookieFy.Includes;

namespace CookieFy.Pages
{
    [Authorize(Roles = "User")]
    public class RegisterRecipe : PageModel
    {
        [BindProperty]
        public IFormFile ImgRecipe { get; set; }

        private readonly CookieDbContext dbContext;

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public List<Tag> Tags { get; set; }
        [BindProperty]
        public List<Ingredient> Ingredientes { get; set; }
        [BindProperty]
        public List<Utensil> Utensils { get; set; }
        
        public RegisterRecipe(CookieDbContext context)
        {
            dbContext = context;
        }

        // recebe a imagem que o user adicionou à receita
        public MemoryStream getImage(IFormFile imageUploaded)
        {

            // define a path do avatar default
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipe.png");

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
        
        // metodo acedido quando o utilizador carrega em criar receita
        [Authorize(Roles = "User")]
        public IActionResult OnPostCreateRecipe()
        {

            TryUpdateModelAsync(this);

            // retorna erros se os campos foram incorretamente preenchidos ou não preenchidos
            if (!ModelState.IsValid) return Page();
            

            User us = dbContext.User.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));
            var now = DateTime.Now;

            DateTime date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            // cria o cliente
            Recipe newRecipe = new Recipe()
            {
                Title = Recipe.Title,
                Description = Recipe.Description,
                Rank = 0,
                Classifications = 0,
                Time = 0,
                Date = date,
                Price = Recipe.Price,
                ImgRecipe = getImage(ImgRecipe).ToArray(),
                UserID = us.UserID
            };

            // adiciona a receita à BD
            dbContext.Recipe.Add(newRecipe);

            foreach(Tag t in Tags)
            {
                Tag newTag = new Tag() { Name = t.Name };
                dbContext.Tag.Add(newTag);

                RecipeTag rt = new RecipeTag()
                {
                    RecipeID = newRecipe.RecipeID,
                    TagID = newTag.TagID,
                };

                dbContext.RecipeTag.Add(rt);
            }

            foreach(Ingredient i in Ingredientes)
            {
                Ingredient newIng = new Ingredient() { Name = i.Name };
                dbContext.Ingredient.Add(newIng);

                RecipeIngredient ri = new RecipeIngredient()
                {
                    RecipeID = newRecipe.RecipeID,
                    IngredientID = newIng.IngredientID,
                };
                dbContext.RecipeIngredient.Add(ri);
            }

            foreach(Utensil u in Utensils)
            {
                Utensil newUten = new Utensil() { Name = u.Name };
                dbContext.Utensil.Add(newUten);

                RecipeUtensils ru = new RecipeUtensils()
                {
                    RecipeID = newRecipe.RecipeID,
                    UtensilID = newUten.UtensilID,
                };
                dbContext.RecipeUtensils.Add(ru);
            }

            // guarda as alterações
            dbContext.SaveChanges();
            var rid = newRecipe.RecipeID;

            return RedirectToPage("./RecipeAddStep", "GetRecipe", new { Recipeid = rid, StepPos = 1}).WithSuccess("Sucesso", "Receita criada com sucesso, adicionar passos", "3000");
        }
    }
}