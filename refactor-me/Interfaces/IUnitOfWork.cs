using refactor_me.Domain;
using refactor_me.Concrete;

namespace refactor_me.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<ProductOption> ProductOptionRepository { get; }
    }
}