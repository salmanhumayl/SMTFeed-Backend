using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Model.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Post1 { get; set; }

     
        public int PostedBy { get; set; }
        public string PostedByName { get; set; }
        public DateTime?  PostDate { get; set; }
        public IFormFile document { get; set; }

        public bool IsAttachment { get; set; }
        public string FilePath { get; set; }

        public virtual AbstractDropDown PostedByNavigation { get; set; }

       
    }

    public class AbstractDropDown 
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
