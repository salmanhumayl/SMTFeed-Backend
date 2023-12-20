using Microsoft.AspNetCore.Hosting;
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

        public AuthenticateController(IUsers userservice, IWebHostEnvironment webHostEnvironmen)
        {
            _userservice = userservice;
            _webHostEnvironment = webHostEnvironmen;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration (UserModel model)
        {
            if (model.TwoFactorCode == null)
            {
                string body;
                string mailcontent;
                string emailResponse;
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
                return Ok(TwoFAtoken);

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
