using System;
using System.Collections.Generic;

#nullable disable

namespace SMT.ModelSQL.Models
{
    public partial class SmtForgetPwdLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Guid { get; set; }
        public string Status { get; set; }
    }
}
