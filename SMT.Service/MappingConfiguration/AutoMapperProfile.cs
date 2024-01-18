using AutoMapper;
using SMT.Common.Model;
using SMT.Model.Models;
using SMT.ModelSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.MappingConfiguration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<SmtForgetPwdLog, ForgetPasswordModel>().ReverseMap();

            CreateMap<PagedData<Post>, PagedData<PostModel>>().ReverseMap();
        }
    }
}
