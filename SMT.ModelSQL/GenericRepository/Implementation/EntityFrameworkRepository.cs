using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMT.ModelSQL.GenericRepository.Repository;
using SMT.ModelSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ModelSQL.GenericRepository.Implementation
{
    public class EntityFrameworkRepository : EntityFrameworkRepositoryReadOnly, IRepository
    {

        private readonly SMTContext _DbContext;
        public EntityFrameworkRepository(SMTContext context, IConfiguration configuration) : base(context, configuration)
        {
            _DbContext = context;

        }

        public void InsertModel<T>(T model) where T : class
        {
            try
            {
                _DbContext.Set<T>().Add(model);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void DeleteModel<T>(int modelId) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<int> ExecuteRowSql(string query)
        {
            return await _DbContext.Database.ExecuteSqlRawAsync(query);
        }

        public async Task<IEnumerable<T>> GetModelAsync<T>() where T : class
        {
            return await _DbContext.Set<T>().ToListAsync();
        }

        public Task<T> GetModelByIdAsync<T>(int modelId) where T : class
        {
            throw new NotImplementedException();
        }

      

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _DbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool UpdateModel<T>(T model) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
