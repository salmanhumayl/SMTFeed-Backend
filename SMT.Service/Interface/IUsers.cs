using SMT.Model.Models;
using SMT.ModelSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.Interface
{
    public interface IUsers
    {
        Task AddUserAsync (UserModel model);

        User FindByNameAsync(string UserName);
        bool CheckPasswordAsync(string UserName, string Password);
        User FindByEmailAsync(string Email);

    }
}
