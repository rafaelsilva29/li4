using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CookieFy.Data;
using CookieFy.Models;
using CookieFy.Includes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CookieFy.Pages
{
    [Authorize(Roles = "User")]
    public class RecipeAddStepModel : PageModel
    {
        private readonly CookieFy.Data.CookieDbContext _context;
        [BindProperty]
        public int Recipeid { get; set; }
        [BindProperty]
        public int StepPos { get; set; }
        [BindProperty]
        public Step Step { get; set; }



        public RecipeAddStepModel(CookieFy.Data.CookieDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public void OnGetRS(int Recipeid, int StepPos)
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Recipe r = _context.Recipe.Find(Recipeid);

            int newtime = r.Time + Step.Time;

            r.Time = newtime;

            Step s = new Step
            {
                Link = Step.Link,
                Position = StepPos,
                Text = Step.Text,
                Time = Step.Time,
                RecipeID = Recipeid
            };

            _context.Entry(r).State = EntityState.Modified;
            _context.Step.Add(s);

            await _context.SaveChangesAsync();
            StepPos++;
            //if (isLast.Equals("finalstep")) return RedirectToPage("./Index");

            return RedirectToPage("./RecipeAddStep", "GetRS", new { recipeid = Recipeid, StepPos = StepPos }).WithSuccess("Sucesso", "Passo criada com sucesso, adicionar próximo", "3000");
        }
    }
}