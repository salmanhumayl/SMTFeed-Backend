using Microsoft.Extensions.DependencyInjection;
using SMT.ModelSQL.GenericRepository.Implementation;
using SMT.ModelSQL.GenericRepository.Repository;
using SMT.Service.Interface;
using SMT.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.Extentions
{
    public static class Dependencyinjections
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, EntityFrameworkRepository>();
        
            services.AddScoped<IPost, PostService>();
            services.AddScoped<IUsers, UserService>();

          

        }
    }
}
