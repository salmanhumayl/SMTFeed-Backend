using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ModelSQL.GenericRepository.Repository
{
   public  interface IRepository : IRepositoryReadOnly
    {
     
        void InsertModel<T>(T model) where T : class;
        void DeleteModel<T>(int modelId) where T : class;
        bool UpdateModel<T>(T model) where T : class;

        Task<int> SaveAsync();

        Task<IEnumerable<T>> GetModelAsync<T>() where T : class;
        Task<T> GetModelByIdAsync<T>(int modelId) where T : class;

        Task<int> ExecuteRowSql(string query);
    }
}
