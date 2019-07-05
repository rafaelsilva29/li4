using System;
namespace CookieFy.Models
{
    public class RecipeOfList
    {
        public int RecipeListID { get; internal set; }
        public int RecipeID { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public RecipeList RecipeList { get; internal set; }
    }
}

