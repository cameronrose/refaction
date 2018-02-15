using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using refactor_me.Interfaces;
using refactor_me.Domain;
using refactor_me.Concrete;
using refactor_me.Tests.Integration.Infrastructure;
using Microsoft.Owin.Testing;
using System;

namespace refactor_me.Tests.Integration
{
    public abstract class BaseServerTest : IDisposable
    {
        protected TestServer _server;
        
        private ITestDatabase _effortDatabaseStrategy;
        private IEntitiesDbContext _context;
        protected static GenericRepository<Product> _productRepository;
        protected static GenericRepository<ProductOption> _productOptionRepository;

        public BaseServerTest()
        {
            // Set up in-memory database
            _effortDatabaseStrategy = CreateTestStrategy();
            _context = _effortDatabaseStrategy.CreateContext();

            // Set up in-memory web server
            _server = TestServer.Create(builder => new TestStartup(_context)
                    .Configuration(builder));            
        }

        protected ITestDatabase CreateTestStrategy()
        {
            return new EffortEntitiesDbContext();
        }

        public void Dispose()
        {
            // dispose of the database and connection
            _effortDatabaseStrategy.Dispose(_context);
            _context = null;

            if (_server != null)
                _server.Dispose();
        }

        protected virtual async Task<TResult> GetAsync<TResult>(string uri)
        {
            var response = await GetAsync(uri);
            return await response.Content.ReadAsAsync<TResult>();
        }

        protected virtual async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var response = await _server.CreateRequest(uri)
                .GetAsync();
            var mastrang = response.Content.ReadAsStringAsync().Result;

            return response;
        }

        protected virtual async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var response = await _server.CreateRequest(uri)
                .SendAsync("DELETE");

            return response;
        }

        protected virtual async Task<HttpResponseMessage> PutAsync<TModel>(object model, string uri)
        {
            return await _server.CreateRequest(uri)
                .And(request => request.Content = new ObjectContent(typeof(object), model, new JsonMediaTypeFormatter()))
                .SendAsync("PUT");
        }
        protected virtual async Task<TModel> PutAsync<TModel>(TModel model, string uri)
        {
            HttpResponseMessage response = await PutAsync<HttpResponseMessage>(model, uri);
            
            return await response.Content.ReadAsAsync<TModel>();
        }

        protected virtual async Task<TModel> PostAsync<TModel>(TModel model, string uri)
        {
            var response = await _server.CreateRequest(uri)
                .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                .PostAsync();
            
            return await response.Content.ReadAsAsync<TModel>();
        }
    }
}
