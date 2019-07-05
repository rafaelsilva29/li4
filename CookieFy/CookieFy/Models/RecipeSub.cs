using System;
namespace CookieFy.Models
{
    public class RecipeSub
    {
        public int UserID { get; internal set; }
        public int RecipeID { get; internal set; }
        public User User { get; internal set; }
        public Recipe Recipe { get; internal set; }
    }
}
