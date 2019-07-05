using System;
namespace CookieFy.Models
{
    public class UserSubscription
    {
        public int UserSubscriptionID { get; set; }

        // ------ Relationships ------ //
        public int? UserID { get; internal set; }
        public int? SubscriptionID { get; internal set; }
        public User User { get; internal set; }
        public Subscription Subscription { get; internal set; }
    }
}
