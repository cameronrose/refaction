using Effort;
using Effort.DataLoaders;
using Effort.Provider;
using System;
using System.Data.Entity.Core.EntityClient;

namespace refactor_me.Tests.Integration.Infrastructure
{ 
    public class BaseConnectionProvider : IDisposable
    {
        protected EntityConnection _connection;

        protected void CreateConnection()
        {
            if (_connection == null)
            {
                // Uncomment the section below to seed Effort with entities from the actual database 
                /*EntityDataLoader loader = new Effort.DataLoaders.EntityDataLoader("name=Entities");
                _connection = EntityConnectionFactory.CreateTransient("name=Entities", loader);*/
                
                _connection = EntityConnectionFactory.CreateTransient("name=Entities");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
