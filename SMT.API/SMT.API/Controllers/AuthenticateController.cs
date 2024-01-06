using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Model.Models;
using SMT.ModelSQL.Models;
using SMT.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VCT.API.Model;

namespace SMT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticateController : ControllerBase
    {
        private readonly IUsers _userservice;
        private IWebHostEnvironment _webHostEnvironment;
        string body;
        string mailcontent;
        string emailResponse;
        public AuthenticateController(IUsers userservice, IWebHostEnvironment webHostEnvironmen)
        {
            _userservice = userservice;
            _webHostEnvironment = webHostEnvironmen;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(UserModel model)
        {
            if (model.TwoFactorCode == null)
            {
                //check Email Already Exist 
                var userByEmailExists = _userservice.FindByEmailAsync(model.Email);
                var userByNameExists = _userservice.FindByNameAsync(model.UserName);

                if (userByEmailExists != null)

                    //   return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "Email already exists!" });
                    return Ok(new Response { Status = "Error", Message = "Email already exists!" });

                if ((userByNameExists != null))
                    //  return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "User already exists!" });
                    return Ok(new Response { Status = "Error", Message = "User already exists!" });


                Random rnd = new Random();
                string TwoFAtoken = (rnd.Next(1000, 9999).ToString());
                //Send OTP Email 

                EmailManager VCTEmailService = new EmailManager();

                body = VCTEmailService.GetBody(Path.Combine(_webHostEnvironment.WebRootPath, @"Template\Registration-2FA-OTP.html"));
                mailcontent = body.Replace("@Name", model.Name);
                mailcontent = mailcontent.Replace("@pwdchangelink", TwoFAtoken);
                VCTEmailService.Body = mailcontent;

                VCTEmailService.Subject = "SMT OTP";
                VCTEmailService.ReceiverAddress = model.Email;
                VCTEmailService.ReceiverDisplayName = model.Email;
                emailResponse = await VCTEmailService.SendEmail();
                return Ok(new Response { Status = "ok", TwoFactorCode = TwoFAtoken });

            }
            else
            {
                if (model.TwoFactorCode == model.GenerateFactorCode)
                    await _userservice.AddUserAsync(model);
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
        }



        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {

            User user = _userservice.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (_userservice.CheckPasswordAsync(user.UserName, model.Password))
                {
                    return Ok(new Iresult<User> { isSuccess = true, data = user });
                }
                else
                {
                    return Ok(new Iresult<User> { isSuccess = false, Message = "Invalid Password" });
                }
            }
            return Ok(new Iresult<User> { isSuccess = false, Message = "Invalid UserName" });

        }

        [HttpGet("Path")]
        public string  ShowPath()
        {
            return _webHostEnvironment.WebRootPath.ToString();



        }

    }


}

    public class Iresult<T>
    {
        public bool isSuccess { get; set; }
       public string Message { get; set; }
        public T data { get; set; }
   
    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
       public string TwoFactorCode { get; set; }
}

