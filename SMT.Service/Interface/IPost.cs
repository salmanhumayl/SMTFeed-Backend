using Microsoft.AspNetCore.Http;
using SMT.Common.Model;
using SMT.Model.Models;
using SMT.ModelSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.Interface
{
    public interface IPost
    {

       
        Task<PagedData<PostModel>> GetPost(int pageNo, int pageSize);


     
        Task<IEnumerable<PostModel>> GetPostAsync();

        Task<IEnumerable<PostModel>> GetPostIncludeAsync();

        Task<int> AddPost(PostModel model);
        Task UpdatePost(PostModel model);
        Task<Post> GetPostById(int id);
        Task<string> ProcessDocument(IFormFile files, string FileName);

        void UpdateFileName(string FileName, int ID);
    }
}
