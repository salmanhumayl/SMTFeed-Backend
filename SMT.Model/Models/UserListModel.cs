using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Model.Models
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SubscriptionValidOn { get; set; }
    }
}
