using Microsoft.AspNetCore.Mvc;
using SMT.Model.Models;
using SMT.ModelSQL.Models;
using SMT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticateController : ControllerBase
    {
        private readonly IUsers _userservice;

        public AuthenticateController(IUsers userservice)
        {
            _userservice = userservice;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration (UserModel model)
        {
            if (model.TwoFactorCode == null)
            {
                Random rnd = new Random();
                string randomnumber = (rnd.Next(1000, 9999).ToString());
                return Ok(randomnumber);
            }
            else
            {
                if (model.TwoFactorCode== model.GenerateFactorCode)
                await _userservice.AddUserAsync(model);
                return Ok();
            }
        }
         


        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {

            User user = _userservice.FindByNameAsync(model.UserName, model.Password);
            if (user != null)
            {
                return Ok(new Iresult<User> {isSuccess = true,data=user});
            }
            return Ok(new Iresult<User> { isSuccess = false });
        }
    }

    public class Iresult<T>
    {
        public bool isSuccess { get; set; }
        public T data { get; set; }
   
    }
}
