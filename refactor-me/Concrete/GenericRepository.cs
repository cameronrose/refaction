using refactor_me.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace refactor_me.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IEntitiesDbContext _context;  
        private DbSet<T> _dbSet;

        public GenericRepository(IEntitiesDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> GetAll()
        { 
            return _dbSet.AsQueryable();
        }

        public T Insert(T obj)
        {
            _dbSet.Add(obj);
            _context.SaveChanges();  
            return obj;
        }

        public async Task<T> InsertAsync(T obj)
        {
            _dbSet.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public int Delete(object id) 
        {
            T entityToDelete = _dbSet.Find(id);
            return Delete(entityToDelete);
        }

        public async Task<int> DeleteAsync(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            return await DeleteAsync(entityToDelete);
        }

        public int Delete(T entityToDelete)
        {
            if (entityToDelete != null)
                _dbSet.Remove(entityToDelete);

            return _context.SaveChanges();
        }

        public async Task<int> DeleteAsync(T entityToDelete)
        {
            if (entityToDelete != null) 
                _dbSet.Remove(entityToDelete);

            return await _context.SaveChangesAsync();
        }

        public T Update(T updatedEntity, object key)
        {
            if (updatedEntity == null)
                return null;

            T existing = _context.Set<T>().Find(key);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updatedEntity);
                _context.SaveChanges();
            }
            return existing;
        }

        public async Task<T> UpdateAsync(T updatedEntity, object key)
        {
            if (updatedEntity == null)
                return null;

            T existing = _context.Set<T>().Find(key);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
            }
            return existing;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}