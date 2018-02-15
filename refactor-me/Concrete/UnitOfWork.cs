using refactor_me.Interfaces;
using refactor_me.Domain;

namespace refactor_me.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEntitiesDbContext _context;
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<ProductOption> _productOptionRepository;

        public UnitOfWork(IEntitiesDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                return _productRepository = _productRepository 
                    ?? new GenericRepository<Product>(_context);
            }
        }

        public IGenericRepository<ProductOption> ProductOptionRepository
        {
            get
            {
                return _productOptionRepository = _productOptionRepository 
                    ?? new GenericRepository<ProductOption>(_context);
            }
        }
    }
}