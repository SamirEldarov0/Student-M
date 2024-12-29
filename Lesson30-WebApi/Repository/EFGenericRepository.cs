using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Repository
{
    public class EFGenericRepository<TEntity,TPrimaryKey>:IGenericRepository<TEntity,TPrimaryKey> where TEntity:BaseEntity<TPrimaryKey>
    {
        private readonly StudentDbContext _context;
        private DbSet<TEntity> Table=>_context.Set<TEntity>();//_context.Students

        public EFGenericRepository(StudentDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> entities = Table;
            return entities;
        }
        private static void BindIncludeProperties(IQueryable<TEntity> query,IEnumerable<Expression<Func<TEntity,object>>> includeProperties)
        {
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAll();
            BindIncludeProperties(query, includeProperties);
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }

        public async Task<List<TEntity>> GetAllList()
        {
            return await GetAll().ToListAsync();
        }

        public Task<List<TEntity>> GetAllListIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAll();
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.ToListAsync();
        }

        public IQueryable<TEntity> FindByIncluding(Expression<Func<TEntity,bool>> predicate,params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAll();
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(predicate);
        }
        public ValueTask<TEntity> Find(TPrimaryKey id)
        {
            var r = Table.FindAsync(id);
            return r;
        }
        //public Task<TEntity> FindEntity(int id)
        //{
        //    TEntity entity=_context.Set<TEntity>().Find(id);
        //    return entity;
        //}

        public Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Table.AnyAsync(predicate);
        }

        public Task<bool> All(Expression<Func<TEntity, bool>> predicate)
        {
            return Table.AllAsync(predicate);
        }

        public Task<int> Count()
        {
            return Table.CountAsync();
        }

        public Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Table.CountAsync(predicate);
        }

        public async Task Add(TEntity entity)
        {
            await Table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            //var entities = _context.Set<TEntity>();
            //entities.Remove(entity);
        }

        public async Task DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = Table.Where(predicate);
            foreach (var item in entities)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }
            //var entities = _context.Set<TEntity>();
            //var query = entities.Where(predicate);
            //entities.RemoveRange(query);
        }

        public async Task Commit(CancellationToken cancellationToken=default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
