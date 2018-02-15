using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace refactor_me.Interfaces
{
    public interface IEntitiesDbContext : IDisposable
    {
        DbContextConfiguration Configuration { get; }
        Database Database { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}