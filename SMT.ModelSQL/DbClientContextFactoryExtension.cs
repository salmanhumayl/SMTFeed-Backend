using Microsoft.EntityFrameworkCore;
using SMT.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ModelSQL
{
    static class DbClientContextFactoryExtension
    {
        internal static async Task<PagedData<Entity>> ToPaginationAsync<Entity>(this IQueryable<Entity> queryable, int page = 1, int pageSize = 10) where Entity : class
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            int skip = pageSize * (page - 1);
            PagedData<Entity> pagination = new PagedData<Entity>
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            int totalRecords = await queryable.CountAsync();
         
            IList<Entity> records = await queryable.Skip(skip).Take(pageSize).ToListAsync();
           
            pagination.NoOfItems = totalRecords;
            pagination.Data = records;
            pagination.NoOfPages = (int)Math.Ceiling((double)pagination.NoOfItems / pageSize);
            return pagination;
        }
    }
}
