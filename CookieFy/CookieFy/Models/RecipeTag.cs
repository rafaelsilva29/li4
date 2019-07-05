using System;
namespace CookieFy.Models
{
    public class RecipeTag
    {
        public int RecipeTagID { get; set; }

        public int? RecipeID { get; internal set; }
        public int? TagID { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public Tag Tag { get; internal set; }
    }
}
