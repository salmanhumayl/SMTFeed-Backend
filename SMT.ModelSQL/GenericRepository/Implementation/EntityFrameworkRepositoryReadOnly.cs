using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMT.Common.Model;
using SMT.ModelSQL.GenericRepository.Repository;
using SMT.ModelSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ModelSQL.GenericRepository.Implementation
{
    public class EntityFrameworkRepositoryReadOnly : IRepositoryReadOnly
    {
        private SMTContext _DbContext;

        public IConfiguration Configuration { get; }
        public EntityFrameworkRepositoryReadOnly(SMTContext context, IConfiguration configuration)
        {
            _DbContext = context;
            Configuration = configuration;
        }
     //  public virtual async Task<PagedData<T>> GetPageModelAsync<T>(int pageNo, int pageSize, Expression<Func<T, bool>> filter = null
       //   , string[] includes = null
       //   ) where T : class {


             public virtual async Task<PagedData<T>> GetPageModelAsync<T>(int pageNo, int pageSize, Expression<Func<T, DateTime>> filter = null, string[] includes = null) where T : class
        { 
            IQueryable<T> dbSet = _DbContext.Set<T>();

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    dbSet = dbSet.Include<T>(item);
                }

            }

            if (filter != null)
            {
                dbSet = dbSet.OrderByDescending(filter);
               // dbSet = dbSet.Where(filter);
            }


            return await dbSet.ToPaginationAsync(pageNo, pageSize);
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
              return _DbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> CheckNumber<T>(Expression<Func<T, DateTime>> filter = null) where T : class
        {
            if (filter != null)
            {
                return await _DbContext.Set<T>().OrderByDescending(filter).ToListAsync();
            }
            return await _DbContext.Set<T>().ToListAsync();
        }
    }
}
