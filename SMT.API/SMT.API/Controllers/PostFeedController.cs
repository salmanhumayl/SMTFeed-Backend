using Microsoft.AspNetCore.Mvc;
using SMT.Common.Model;
using SMT.Model.Models;
using SMT.Service.Interface;
using SMT.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostFeedController : ControllerBase
    {
        private IPost _PostService;



        public PostFeedController(IPost PostService)
        {
            _PostService = PostService;
        
        }
        [HttpPost ("AddPost")]
        public async Task<IActionResult> AddPost([FromForm] PostModel model)
        {

            model.PostedBy = 1;
            int _id = await _PostService.AddPost(model);
            if (_id > 0)
            {
                if (model.document != null)
                {
                    var res = await _PostService.ProcessDocument(model.document, "_id");
                    if (res != null)
                    {
                        _PostService.UpdateFileName(res, _id);
                    }
                }

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
