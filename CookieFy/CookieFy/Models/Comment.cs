using System;
namespace CookieFy.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        public string Text { get; set; } // comment text

        public int Likes { get; set; } // comment likes

        // ------ Relationships ------ //
        public int? UserID { get; set; }
        public int? RecipeID { get; set; }
        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}

