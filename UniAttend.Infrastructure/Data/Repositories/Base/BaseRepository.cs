using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Base;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        protected BaseRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet.FindAsync(new object[] { id }, cancellationToken);

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await DbSet.ToListAsync(cancellationToken);

        public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(e => e.Id == id, cancellationToken);

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}