using AutoMapper;
using SMT.Model.Models;
using SMT.ModelSQL.GenericRepository.Repository;
using SMT.ModelSQL.Models;
using SMT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.Service
{
    public class UserService : IUsers
    {

        public readonly IRepository _repository;
        public readonly IMapper _iMapper;
        public UserService(IRepository iRepository,IMapper iMapper)
        {
            _repository = iRepository;
            _iMapper = iMapper;
        }
        public async Task AddUserAsync(UserModel model)
        {
            User Usermodel = _iMapper.Map<User>(model);
            Usermodel.SubscriptionValidOn = DateTime.Now.AddDays(30);
            _repository.InsertModel(Usermodel);
            await _repository.SaveAsync();
        }
        
        public bool CheckPasswordAsync(string UserName, string Password)
        {
            var user= _repository.GetQueryable<User>().Where(a => a.UserName == UserName && a.Password==Password).SingleOrDefault();
            if (user != null)
                return true;
            else
                return false;
        }

        public async Task<User> FindByEmailAsync(string Email)
        {
            var dccUsers = _repository.GetQueryable<User>().Where(a => a.Email ==Email).SingleOrDefault();
            return await Task.FromResult(dccUsers);
        }

        public async Task<User> FindByIdAsync(int UserId)
        {
            var dccUsers = _repository.GetQueryable<User>().Where(a => a.Id == UserId).SingleOrDefault();
            return await Task.FromResult(dccUsers);
        }

        public async Task<User> FindByNameAsync(string UserName)
        {
            var dccUsers = _repository.GetQueryable<User>().Where(a => a.UserName == UserName).SingleOrDefault();
            return await Task.FromResult(dccUsers);
        }

        public async Task<int> ForgetPasswrod(ForgetPasswordModel forgetpasswordmodel)
        {

           SmtForgetPwdLog model = _iMapper.Map<SmtForgetPwdLog>(forgetpasswordmodel);
            _repository.InsertModel(model);
            await _repository.SaveAsync();
            return 1; 

           
        }

        public async Task<IEnumerable<UserListModel>> GetUsers()
        {
            IEnumerable<User> user = await _repository.GetModelAsync<User>();
            return _iMapper.Map<IEnumerable<UserListModel>>(user);
        }

        public SmtForgetPwdLog PwdLogUserInfo(string token)
        {
            var dccUsers = _repository.GetQueryable<SmtForgetPwdLog>().Where(a => a.Guid == token && a.Status == "X").SingleOrDefault();
            return dccUsers;
          
        }

        public async Task<int> UpdatePassword(int Id, string NewPassword)
        {
            return await _repository.ExecuteRowSql("Update Users Set Password='" + NewPassword + "' Where ID =" + Id);

        }

        public async Task<int> UpdatePasswordStatus(int ID)
        {
            return await _repository.ExecuteRowSql("Update SMT_ForgetPwdLog Set Status='C' Where ID =" + ID);
            
        }

        
    }
}
