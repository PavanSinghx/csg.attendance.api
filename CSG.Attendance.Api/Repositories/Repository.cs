using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public class Repository<TEntityType> : IRepository<TEntityType> where TEntityType : class, new()
    {
        private readonly DbContext databaseContext;

        public Repository(DbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task AddAsync(TEntityType data)
        {
            await this.databaseContext.Set<TEntityType>().AddAsync(data);
            await SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntityType> dataList)
        {
            await this.databaseContext.Set<TEntityType>().AddRangeAsync(dataList);
            await SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntityType, bool>> predicate)
        {
            return this.databaseContext.Set<TEntityType>().AnyAsync(predicate);
        }

        public Task RemoveAsync(TEntityType data)
        {
            this.databaseContext.Set<TEntityType>().Remove(data);
            return SaveChangesAsync();
        }

        public Task RemoveRangeAsync(IEnumerable<TEntityType> dataList)
        {
            this.databaseContext.Set<TEntityType>().RemoveRange(dataList);
            return SaveChangesAsync();
        }

        public Task Update(TEntityType data)
        {
            this.databaseContext.Set<TEntityType>().Update(data);
            return SaveChangesAsync();
        }

        public Task UpdateRangeAsync(IEnumerable<TEntityType> dataList)
        {
            this.databaseContext.Set<TEntityType>().UpdateRange(dataList);
            return SaveChangesAsync();
        }

        public Task<TEntityType> FirstOrDefaultAsync(Expression<Func<TEntityType, bool>> predicate)
        {
            return this.databaseContext.Set<TEntityType>().FirstOrDefaultAsync(predicate);
        }

        public Task<List<TEntityType>> GetAllAsync(Expression<Func<TEntityType, bool>> predicate)
        {
            return this.databaseContext.Set<TEntityType>().Where(predicate).ToListAsync();
        }

        private Task<int> SaveChangesAsync()
        {
            return this.databaseContext.SaveChangesAsync();
        }
    }
}
