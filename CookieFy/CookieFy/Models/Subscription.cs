using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookieFy.Models
{
    public class Subscription
    {
        // Props
        public int SubscriptionID { get; set; }

        [Required]
        public string Name { get; set; } = "Free";

        [Required]
        public double Price { get; set; } = 0;

        // ------ Relationships ------ //
        public List<UserSubscription> UserSubscriptions { get; set; }
    }
}

