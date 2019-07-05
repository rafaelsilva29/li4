using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookieFy.Models
{
    public class User
    {

        // Props
        public int UserID { get; set; } // id to identify an user in db

        [Required] // model validation
        [StringLength(30)]
        public string Name { get; set; } // user name

        [Required]
        [StringLength(200)]
        public string Profession { get; set; } // user profession

        [Required]
        [StringLength(500)]
        public string Country { get; set; } // user address -> country

        [Required]
        [StringLength(500)]
        public string City { get; set; } // user address -> city

        [Required]
        public double Balance { get; set; } = 0;// user money

        [Required]
        [StringLength(500)]
        public string Email { get; set; } // user email

        public byte[] ImgPath { get; set; } = new byte[1024]; //  user profile pic path
    
        [Required]  
        [StringLength(500)]     
        public string Description { get; set; } // user description

        [Required]
        public string Password { get; set; }

        //public string PasswordSalt { get; set; } // for encrypt

        // ------ Relationships ------ //
        // Comments
        public List<Comment> Comments { get; set; }
        // Recipes
        public List<Recipe> Recipes { get; set; }
        // Follows
        public List<User> Follows { get; set; }
        public User UserFollow { get; set; }
        // WeekPlan
        public List<RecipePlan> RecipesPlan { get; set; }
        // RecipeList
        public List<RecipeList> RecipeLists { get; set; }
        // Subscription
        public List<UserSubscription> Subscriptions { get; set; }
    }
}
