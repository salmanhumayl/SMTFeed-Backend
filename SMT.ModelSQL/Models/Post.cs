using System;
using System.Collections.Generic;

#nullable disable

namespace SMT.ModelSQL.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Post1 { get; set; }
        public DateTime  PostDate { get; set; }
        public int PostedBy { get; set; }
        public bool IsAttachment { get; set; }
        public bool? IsActive { get; set; }
        public string FilePath { get; set; }

        public virtual User PostedByNavigation { get; set; }
    }
}
