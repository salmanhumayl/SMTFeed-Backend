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
            _repository.InsertModel(Usermodel);
            await _repository.SaveAsync();
        }

        public User FindByNameAsync(string UserName, string Password)
        {
            var dccUsers = _repository.GetQueryable<User>().Where(a => a.UserName == UserName && a.Password == Password).SingleOrDefault();

            return dccUsers;
        }
    }
}
