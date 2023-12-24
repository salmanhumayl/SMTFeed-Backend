using AutoMapper;
using Microsoft.AspNetCore.Http;
using SMT.Common.Model;
using SMT.Model.Models;
using SMT.ModelSQL.GenericRepository.Repository;
using SMT.ModelSQL.Models;
using SMT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SMT.Service.Service
{
    public class PostService : IPost
    {
        private readonly IRepository _repository;
        private IMapper _iMapper;
        private IFileUtilityService _fileUtilityService;

        public PostService(IRepository repository, IMapper iMapper,IFileUtilityService fileUtilityService)
        {
            _repository = repository;
            _iMapper = iMapper;
            _fileUtilityService = fileUtilityService;


        }

        public async Task<int> AddPost(PostModel Postmodel)
        {
            Post model = _iMapper.Map<Post>(Postmodel);
            model.PostDate = DateTime.Now;
            _repository.InsertModel(model);
            await _repository.SaveAsync();
            return model.Id;
        }


        public async Task<PagedData<PostModel>> GetPost(int pageNo, int pageSize)  
        {
            PagedData<Post> Posts = await _repository.GetPageModelAsync<Post>(pageNo, pageSize, null, new string[] {"PostedByNavigation"});
            
            //PagedData<Post> Posts = await _repository.GetPageModelAsync<Post>(pageNo, pageSize, a => a.PostDate);
            return _iMapper.Map<PagedData<PostModel>>(Posts);
        }


        // public async Task<IEnumerable<JournalViewModel>> GetPostAsync()
        //   {
        //       IEnumerable<Journal> Journals = await _repository.GetModelAsync<Journal>();
        //       return _iMapper.Map<IEnumerable<JournalViewModel>>(Journals);
        //    }


  

        public async Task<IEnumerable<PostModel>> GetPostAsync()
        {

             IEnumerable<Post> posts = await _repository.GetModelAsync<Post>();
            return _iMapper.Map<IEnumerable<PostModel>>(posts);
        }

        public Task<Post> GetPostById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PostModel>> GetPostIncludeAsync()
        {
            var data = _repository.GetQueryable<Post>().Include(p =>p.PostedByNavigation);
           

            var post = _repository.GetQueryable<Post>();
            var user = _repository.GetQueryable<User>();


            var result = (from pp in post
                          join u in user on pp.PostedBy equals u.Id
                          orderby pp.PostDate descending
                          select new PostModel
                          {
                              Post1 = pp.Post1,
                              PostedByName = u.Name,
                              PostDate=pp.PostDate

                          }).ToListAsync().ConfigureAwait(false);

            return await result;

        }

        public async  Task<string> ProcessDocument(IFormFile files,string fileName)
        {
            var filePath = await _fileUtilityService.WriteIFormFileOnNetwork(files, fileName);
            return filePath;
        }
        public void UpdateFileName(string FileName, int ID)
        {
            _repository.ExecuteRowSql("Update Post Set Path='" + FileName + "' Where id =" + ID);
        }
        public Task UpdatePost(PostModel model)
        {
            throw new NotImplementedException();
        }



    }
}
