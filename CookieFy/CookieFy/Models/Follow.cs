using System;
namespace CookieFy.Models
{
    public class Follow
    {
        public int FollowID { get; set; }
        public User User { get; internal set; }
    }
}
