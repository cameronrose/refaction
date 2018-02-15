using System.Linq;
using System.Threading.Tasks;

namespace refactor_me.Concrete
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Insert(T obj);
        Task<T> InsertAsync(T obj);
        int Delete(object Id);
        Task<int> DeleteAsync(object Id);
        T Update(T obj, object key);
        Task<T> UpdateAsync(T obj, object key);
    }
}