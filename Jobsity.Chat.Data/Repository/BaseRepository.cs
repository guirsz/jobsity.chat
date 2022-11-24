using Jobsity.Chat.Data.Context;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MyContext context;

        protected DbSet<T> dataset;

        public BaseRepository(MyContext context)
        {
            this.context = context;
            dataset = context.Set<T>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await dataset.SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (result == null)
                return false;

            result.Deactivated = true;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<T> InsertAsync(T entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.CreateAt = DateTime.UtcNow;
            entity.Deactivated = false;

            dataset.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            return await dataset.AnyAsync(X => X.Id.Equals(id));
        }

        public async Task<T> SelectAsync(Guid id)
        {
            return await dataset.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await dataset.Where(x => x.Deactivated == false).ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = await dataset.SingleOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (result == null)
                return null;

            entity.UpdateAt = DateTime.UtcNow;
            entity.CreateAt = result.CreateAt;
            entity.Deactivated = false;

            context.Entry(result).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}