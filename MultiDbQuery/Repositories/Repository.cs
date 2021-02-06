using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MultiDbQuery.Repositories
{
    public class Repository<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context { get; }

        public Repository(TDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<TEntity>> GetAll<TEntity>(TEntity entity) where TEntity:class
        {
            await Task.Delay(4);
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> Find<TEntity>(string id) where TEntity:class
        {
            await Task.Delay(4);
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Attach(entity);
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if(_context.Entry(entity).State == EntityState.Detached)
                _context.Attach(entity);

            await Task.Delay(TimeSpan.FromMilliseconds(1));
            _context.Remove(entity);
        }
    }
}
