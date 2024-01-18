using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;

        string body;
        string mailcontent;
        string emailResponse;
        public AuthenticateController(IUsers userservice, IWebHostEnvironment webHostEnvironmen, IConfiguration configuration)
        {
            _userservice = userservice;
            _webHostEnvironment = webHostEnvironmen;
            _configuration = configuration;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(UserModel model)
        {
            if (model.TwoFactorCode == null)
            {
                //check Email Already Exist 
                var userByEmailExists = await _userservice.FindByEmailAsync(model.Email);
                var userByNameExists = await _userservice.FindByNameAsync(model.UserName);

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
        public async Task<IActionResult> Login(LoginModel model)
        {

            User user = await _userservice.FindByNameAsync(model.UserName);
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



        [HttpPost]
        [Route("forgetpwd")]
        public async Task<IActionResult> forgetpassword(ForgetPasswordEmaiModel forgetmodel)
        {

            var userByEmailExists = await _userservice.FindByEmailAsync(forgetmodel.EmailAddress);

            if (userByEmailExists == null)
                //return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "Email does not exist!" });
                return Ok(new Response { Status = "Error",Message= "Email does not exist!" });
            string mGuid = Guid.NewGuid().ToString();
          

            ForgetPasswordModel model = new ForgetPasswordModel();
            model.Guid = mGuid;
            model.UserID = userByEmailExists.Id;
            var response = await _userservice.ForgetPasswrod(model);
        
            if (response == 1) //Send EMAIL TO User 
            {

                string url = _configuration["ForgetPassword:Url"];
                string Link = url + "?token=" + mGuid;
                EmailManager VCTEmailService = new EmailManager();
                VCTEmailService.Configuration = _configuration;
                string body = VCTEmailService.GetBody(Path.Combine(_webHostEnvironment.WebRootPath, @"Template\ForgetPassword.html"));
                mailcontent = body.Replace("@pwdchangelink", Link); //Replace Contenct...
                mailcontent = mailcontent.Replace("@Name", userByEmailExists.Name); //Replace Contenct...
                VCTEmailService.Body = mailcontent;
                VCTEmailService.Subject = _configuration["ForgetPassword:Subject"];
                VCTEmailService.ReceiverAddress = forgetmodel.EmailAddress;
                VCTEmailService.ReceiverDisplayName = userByEmailExists.Name;
                emailResponse = await VCTEmailService.SendEmail();
                return Ok(new Response { Status = "Success" ,Message= "Instruction to reset your password were sent to your registered email." });
            }
            return Ok("Error");
        }


        [Route("forgetpwdupdate"), HttpPost] //Reset Password
        public async Task<IActionResult> forgetpwdupdate(ChangePasswordViewModel model)
        {
            var forgetinfo = _userservice.PwdLogUserInfo(model.token);
            if (forgetinfo != null)
            {
                var user = await _userservice.FindByIdAsync(forgetinfo.UserId);
                var result = await _userservice.UpdatePassword(user.Id,model.NewPassword);
                if (result > 0 )
                {
                    var response = await _userservice.UpdatePasswordStatus(forgetinfo.Id);
                    EmailManager EmailService = new EmailManager();
                    EmailService.Configuration = _configuration;
                    string body = EmailService.GetBody(Path.Combine(_webHostEnvironment.WebRootPath, @"Template\ChangePassword.html"));
                    EmailService.Body = body.Replace("@Name", user.Name); //Replace Contenct.
                    EmailService.Subject = _configuration["ChangePassword:Subject"];
                    EmailService.ReceiverAddress = user.Email;
                    EmailService.ReceiverDisplayName = user.Email;
                    emailResponse = await EmailService.SendEmail();
                    return Ok(new Response { Status = "Success", Message = "successfully!" });
                }
                
            }

            //   return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Link has been expired / Password has been Updated...!" });
            return Ok(new Response { Status = "Error", Message = "Link has been expired / Password has been Updated...!" });

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

