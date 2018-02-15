using refactor_me.Interfaces;
using refactor_me.Domain;

namespace refactor_me.Tests.Integration.Infrastructure
{
    public class EffortEntitiesDbContext :
        BaseConnectionProvider, ITestDatabase
    {
        public IEntitiesDbContext CreateContext()
        {
            // Create connection to Effort
            base.CreateConnection();

            // Use the Effort connection to instantiate EntitiesDbConext
            var context = new EntitiesDbContext(_connection);

            return context;
        }

        public void Dispose(IEntitiesDbContext context)
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
