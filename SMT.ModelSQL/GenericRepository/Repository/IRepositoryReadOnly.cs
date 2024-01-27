using SMT.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ModelSQL.GenericRepository.Repository
{
  public interface IRepositoryReadOnly
    {
       //  Task<PagedData<T>> GetPageModelAsync<T>(int pageNo, int pageSize, Expression<Func<T, bool>> filter = null, string[] includes = null) where T : class;

       IQueryable<T> GetQueryable<T>() where T : class;
       Task<PagedData<T>> GetPageModelAsync<T>(int pageNo, int pageSize, Expression<Func<T, DateTime>> filter = null, string[] includes = null) where T : class;
    }
}
