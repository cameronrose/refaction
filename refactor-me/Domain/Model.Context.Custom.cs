namespace refactor_me.Domain
{
    using Interfaces;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Infrastructure;

    public partial class EntitiesDbContext : DbContext, IEntitiesDbContext
    {
        public EntitiesDbContext(EntityConnection connection)
            : base(connection, true)
        {
        }
    }
}