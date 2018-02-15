using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using refactor_me.ViewModels;

namespace refactor_me.Tests.Integration
{
    [Trait("Category", "Integration Tests")]
    public partial class ProductOptionsTests : BaseServerTest, IAsyncLifetime
    {
        ProductViewModel _product;
        ProductOptionViewModel _productOption;
        const string ProductOptionName = "Test Product Option";

        public async Task InitializeAsync()
        {
            var product = new ProductViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Price = 1000,
                DeliveryPrice = 16
            };

            _product = await PostAsync(product, "/products");

            _productOption = new ProductOptionViewModel
            {
                Id = Guid.NewGuid(),
                Name = ProductOptionName,
                Description = "Some description"
            };
        }

        public async Task DisposeAsync()
        {
            await DeleteAsync($"/products/{_product.Id}");
        }

        [Fact]
        public async void get_many_empty_product_options()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await GetAsync<ProductItemsViewModel>($"/products/{invalidId}/options");

            // Assert
            Assert.NotNull(response.Items);
            Assert.Empty(response.Items);
        }

        [Fact]
        public async void post_then_get_product_option()
        {
            // Act
            var postResponse = await PostAsync(_productOption, $"/products/{_product.Id}/options");

            var getResponse = await GetAsync<ProductOptionViewModel>
                ($"/products/{_product.Id}/options/{_productOption.Id}");

            // Assert
            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(postResponse.Id, getResponse.Id);
        }

        [Fact]
        public async void get_invalid_product_option_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var getResponse = await GetAsync($"/products/{_product.Id}/options/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async void post_then_get_all_product_options()
        {
            // Act
            var postResponse = await PostAsync(_productOption, $"/products/{_product.Id}/options");

            var getResponse = await GetAsync<ProductOptionItemsViewModel>
                ($"/products/{_product.Id}/options");

            // Assert
            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(_productOption.Name, postResponse.Name);

            Assert.NotEmpty(getResponse.Items);
            Assert.Equal(postResponse.Id, getResponse.Items.Single().Id);
            Assert.Equal(postResponse.Name, getResponse.Items.Single().Name);
        }

        [Fact]
        public async void post_then_put_then_get_product_option()
        {
            // Act
            var postResponse = await PostAsync(_productOption, $"/products/{_product.Id}/options");

            _productOption.Name = "Updated name";
            var putResponse = await PutAsync(_productOption, $"/products/{_product.Id}/options/{_productOption.Id}");

            var getResponse = await GetAsync<ProductOptionViewModel>
                ($"/products/{_product.Id}/options/{_productOption.Id}");

            // Assert
            Assert.Equal(ProductOptionName, postResponse.Name);
            Assert.Equal(_productOption.Name, putResponse.Name);
            Assert.Equal(_productOption.Name, getResponse.Name);

            Assert.NotEqual(Guid.Empty, postResponse.Id);
            Assert.Equal(postResponse.Id, putResponse.Id);
            Assert.Equal(postResponse.Id, getResponse.Id);
        }

        [Fact]
        public async void put_invalid_product_option_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var putResponse = await PutAsync<HttpResponseMessage>
                (_productOption, $"/products/{_product.Id}/options/{_productOption.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Fact]
        public async void post_then_delete_product_option()
        {
            // Act
            var postResponse = await PostAsync(_productOption, $"/products/{_product.Id}/options");
            var deleteResponse = await DeleteAsync($"/products/{_product.Id}/options/{_productOption.Id}");
            var getResponse = await GetAsync($"/products/{_product.Id}/options/{_productOption.Id}");

            // Assert
            Assert.Equal(_productOption.Name, postResponse.Name);
            Assert.NotEqual(Guid.Empty, postResponse.Id);

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async void delete_invalid_product_option_id()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act
            var deleteResponse = await DeleteAsync($"/products/{_product.Id}/options/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
    }
}
