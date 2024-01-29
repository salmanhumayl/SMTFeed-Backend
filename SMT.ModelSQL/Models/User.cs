using System;
using System.Collections.Generic;

#nullable disable

namespace SMT.ModelSQL.Models
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SubscriptionValidOn { get; set; }
        public string UserType { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
