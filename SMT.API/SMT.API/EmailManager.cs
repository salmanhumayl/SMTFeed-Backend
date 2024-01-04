using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace VCT.API.Model
{
    public class EmailManager
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverDisplayName { get; set; }
        public  IConfiguration Configuration { get; set; }
        public string EnvelopID { get; set; }
        public bool Attachement { get; set; }
      
           
        public Task<string> SendEmail()
        {
            System.Net.Mail.MailMessage newemail = new System.Net.Mail.MailMessage();
            System.Net.Mail.MailAddress MailReceiver = new System.Net.Mail.MailAddress(ReceiverAddress, ReceiverDisplayName);
            System.Net.Mail.MailAddress MailSender = new System.Net.Mail.MailAddress("info@supermcxtip.com", "SMT");
            newemail.From = MailSender;
            newemail.To.Add(MailReceiver);
            newemail.IsBodyHtml = true;
            newemail.Subject = Subject;
            newemail.Body = Body;
            return SendMail(newemail);
        }

        private async Task<string> SendMail(System.Net.Mail.MailMessage MailMsg)
        {
            System.Net.Mail.SmtpClient newe = new System.Net.Mail.SmtpClient();
              newe.Host = "supermcxtip.com";

          //  newe.Host = "sg2nlvphout-v01.shr.prod.sin2.secureserver.net"; 
            newe.Port = 25;
            newe.Credentials = new System.Net.NetworkCredential("info@supermcxtip.com", "smt123@@"); //sehprusmlyozedjm
            newe.UseDefaultCredentials = false ;
            newe.EnableSsl = false;
            try
            {
                await newe.SendMailAsync(MailMsg);
            }
            catch (Exception e)
            {
                return e.Message;
                newe.Dispose();
            }
            newe.Dispose();
            return "";

        }


        public string GetBody(string TemplatePath)
        {
            StreamReader sr;
            sr = new StreamReader(TemplatePath);
            string _EmailBody = sr.ReadToEnd();
            sr.Dispose();
            return _EmailBody;
        }
    }
}
