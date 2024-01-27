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

        Task<User> FindByIdAsync(int UserId);
        Task<User> FindByNameAsync(string UserName);
        bool CheckPasswordAsync(string UserName, string Password);
        Task<User> FindByEmailAsync(string Email);

        SmtForgetPwdLog PwdLogUserInfo(string token);
        Task<int> ForgetPasswrod(ForgetPasswordModel model);

        Task<int>UpdatePassword(int Id, string NewPassword);
        Task<int> UpdatePasswordStatus(int ID);

        Task<IEnumerable<UserListModel>> GetUsers();

    }
}
