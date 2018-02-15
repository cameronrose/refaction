using Autofac;
using AutoMapper;
using refactor_me.Domain;
using refactor_me.Interfaces;
using refactor_me.Concrete;

namespace refactor_me.Tests.Integration
{
    public class TestStartup : Startup
    {
        private static IEntitiesDbContext _context;

        public TestStartup(IEntitiesDbContext context)
        {
            _context = context;
        }

        public override void RegisterTypes(ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            // Create a mapper that will be used by the DI container
            var mapper = config.CreateMapper();

            // Register the DI interfaces with their implementation
            builder.RegisterInstance(mapper);

            builder.RegisterInstance(_context)
                .As<IEntitiesDbContext>();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();
        } 
    } 
}
