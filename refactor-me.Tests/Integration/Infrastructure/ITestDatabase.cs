using refactor_me.Interfaces;
using System;

namespace refactor_me.Tests.Integration.Infrastructure
{
    public interface ITestDatabase : IDisposable
    {
        IEntitiesDbContext CreateContext();

        void Dispose(IEntitiesDbContext context);
    }
}
