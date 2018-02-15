using Xunit;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using refactor_me.ViewModels;

namespace refactor_me.Tests.Integration
{
    [Trait("Category", "Integration Tests")]
    public partial class ProductTests : BaseServerTest
    {
        ProductViewModel _product;
        const string ProductName = "Test Product";

        public ProductTests()
        {
            _product = new ProductViewModel
            {
                Id = Guid.NewGuid(),
                Name = ProductName,
                Description = "Some description",
                Price = 1000,
                DeliveryPrice = 16
            };
        }

        [Fact]
        public async void get_many_empty_products()
        {
            // Act
            var response = await GetAsync<ProductItemsViewModel>("/products/");

            // Assert
            Assert.NotNull(response.Items);
            Assert.Empty(response.Items);
        }

        [Fact]
        public async void post_then_get_product()
        {
            // Act
            var postResponse = await PostAsync(_product, "/products");
            var getResponse = await GetAsync<ProductViewModel>($"/products/{_product.Id}");

            // Assert
            Assert.Equal(_product.Name, postResponse.Name);
            Assert.Equal(_product.Name, getResponse.Name);

            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(postResponse.Id, getResponse.Id);
        }

        [Fact]
        public async void post_then_get_product_by_name()
        {
            // Act
            var postResponse = await PostAsync(_product, "/products");
            var getResponse = await GetAsync<ProductItemsViewModel>($"/products?name={_product.Name}");

            // Assert
            Assert.Equal(_product.Name, postResponse.Name);
            Assert.Equal(_product.Name, getResponse.Items.Single().Name);

            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(postResponse.Id, getResponse.Items.Single().Id);
        }

        [Fact]
        public async void get_invalid_product_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var getResponse = await GetAsync($"/products/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async void post_then_get_all_products()
        {
            // Act
            var postResponse = await PostAsync(_product, "/products");
            var getResponse = await GetAsync<ProductItemsViewModel>("/products/");

            // Assert
            Assert.Equal(_product.Name, postResponse.Name);
            Assert.NotEqual(Guid.Empty, postResponse.Id);

            Assert.NotEmpty(getResponse.Items);
            Assert.Equal(postResponse.Id, getResponse.Items.Single().Id);
            Assert.Equal(postResponse.Name, getResponse.Items.Single().Name);
        }

        [Fact]
        public async void post_then_put_then_get_product()
        {
            // Act
            var postResponse = await PostAsync(_product, "/products");
            _product.Name = "Updated name";
            var putResponse = await PutAsync(_product, $"/products/{_product.Id}");
            var getResponse = await GetAsync<ProductViewModel>($"/products/{_product.Id}");

            // Assert
            Assert.Equal(ProductName, postResponse.Name);
            Assert.Equal(_product.Name, putResponse.Name);
            Assert.Equal(_product.Name, getResponse.Name);

            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(postResponse.Id, putResponse.Id);
            Assert.Equal(postResponse.Id, getResponse.Id);
        }

        [Fact]
        public async void put_invalid_product_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var putResponse = await PutAsync<HttpResponseMessage>(_product, $"/products/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Fact]
        public async void post_then_delete_product()
        {
            // Act
            var postResponse = await PostAsync(_product, "/products");
            var deleteResponse = await DeleteAsync($"/products/{_product.Id}");
            var getResponse = await GetAsync($"/products/{_product.Id}");

            // Assert
            Assert.Equal("Test Product", postResponse.Name);
            Assert.NotEqual(Guid.Empty, postResponse.Id);

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async void delete_invalid_product_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var deleteResponse = await DeleteAsync($"/products/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
    }

}
