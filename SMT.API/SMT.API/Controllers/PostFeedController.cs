using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMT.Common.Model;
using SMT.Model.Models;
using SMT.Service.Interface;
using SMT.Service.Service;
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
    public class PostFeedController : ControllerBase
    {
        private IPost _PostService;
        private IWebHostEnvironment _webHostEnvironment;


        public PostFeedController(IPost PostService, IWebHostEnvironment webHostEnvironmen)
        {
            _PostService = PostService;
            _webHostEnvironment = webHostEnvironmen;

        }
        [HttpPost ("AddPost")]
        public async Task<IActionResult> AddPost([FromForm] PostModel model)
        {
            string body;
            string mailcontent;
            EmailManager VCTEmailService = new EmailManager();

            int _id = await _PostService.AddPost(model);
            if (_id > 0)
            {
                if (model.document != null)
                {
                    var res = await _PostService.ProcessDocument(model.document,_id.ToString());
                    if (res != null)
                    {
                  //      _PostService.UpdateFileName(res, _id);
                    }
                }

                //Send An Email to Administrator for newly added Post 


                body = VCTEmailService.GetBody(Path.Combine(_webHostEnvironment.WebRootPath, @"Template\NewPostAdded.html"));
                mailcontent = body.Replace("@Content", model.Post1);
                VCTEmailService.Body = mailcontent;

                VCTEmailService.Subject = "SuperMcxtip - Newly Added Post";
                VCTEmailService.ReceiverAddress = "info@supermcxtip.com";
                VCTEmailService.ReceiverDisplayName = "Supermcxtip";
                await VCTEmailService.SendEmail();
                return Ok(new { message ="ok" });
            }

            else
            {
                // return BadRequest(new { message = "Error while saving record" });
                return Ok(new { message = "Error while saving record" });
            }
        }


        [HttpGet("GetPost")]
        public async Task<IActionResult> GetPost(int pageNumber)
        {
            PagedData<PostModel> posts = await _PostService.GetPost(pageNumber, 10);

       
            return Ok(posts);
        }

        [HttpGet("GetPostAsync")]
        public IActionResult Get()
        {
            var Journals = _PostService.GetPostAsync();
          
            return Ok(Journals);
        }



        

        [HttpGet("GetPostInclude")]
        public async Task<IActionResult> GetPostInclude()
        {
            var posts = await _PostService.GetPostIncludeAsync();

            return Ok(posts);
        }

    }

   
}
