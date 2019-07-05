using System;
namespace CookieFy.Models
{
    public class RecipeUtensils
    {
        public int RecipeUtensilsID { get; set; }
        public int? RecipeID { get; internal set; }
        public int? UtensilID { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public Utensil Utensil { get; internal set; }
    }
}
