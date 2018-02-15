using System;
using System.Web.Http;
using refactor_me.Domain;
using System.Linq;
using System.Collections.Generic;
using refactor_me.Interfaces;
using log4net;
using System.Threading.Tasks;
using System.Data.Entity;
using refactor_me.ViewModels;
using AutoMapper;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProductsController));
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _log.Debug("Entering ProductsController()");
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _unitOfWork.ProductRepository.GetAll().ToListAsync();
            var viewModel = ToModel<IEnumerable<Product>, ProductItemsViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("name={name}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            var result = await _unitOfWork.ProductRepository.GetAll()
                .Where(m => m.Name.Equals(name)).ToListAsync();

            var viewModel = ToModel<IEnumerable<Product>, ProductItemsViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            var result = await _unitOfWork.ProductRepository.GetAll()
                .SingleOrDefaultAsync(m => m.Id.Equals(id));

            var viewModel = ToModel<Product, ProductViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]ProductViewModel productViewModel)
        {
            var product = ToModel<ProductViewModel, Product>(productViewModel);
            var result = await _unitOfWork.ProductRepository.InsertAsync(product);
            var viewModel = ToModel<Product, ProductViewModel>(result);

            return Ok(viewModel);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update(Guid id, [FromBody]ProductViewModel productViewModel)
        {
            var product = ToModel<ProductViewModel, Product>(productViewModel);
            var result = await _unitOfWork.ProductRepository.UpdateAsync(product, id);
            var viewModel = ToModel<Product, ProductViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.ProductRepository.DeleteAsync(id);

            if (result > 0)
                return Ok();

            return NotFound();
        }

        [Route("{productId}/options")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOptions(Guid productId)
        {
            var result = await _unitOfWork.ProductOptionRepository.GetAll()
                .Where(m => m.ProductId == productId).ToListAsync();

            var viewModel = ToModel<IEnumerable<ProductOption>, ProductOptionItemsViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOption(Guid productId, Guid id)
        {
            var result = await _unitOfWork.ProductOptionRepository.GetAll()
                .SingleOrDefaultAsync(m => m.ProductId.Equals(productId) && m.Id.Equals(id));

            var viewModel = ToModel<ProductOption, ProductOptionViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateOption(Guid productId, [FromBody]ProductOptionViewModel optionViewModel)
        {
            var option = ToModel<ProductOptionViewModel, ProductOption>(optionViewModel);
            option.ProductId = productId;
            var result = await _unitOfWork.ProductOptionRepository.InsertAsync(option);
            var viewModel = ToModel<ProductOption, ProductOptionViewModel>(result);
            
            return HttpResult(viewModel);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateOption(Guid productId, Guid id, [FromBody]ProductOptionViewModel optionViewModel)
        {
            var option = ToModel<ProductOptionViewModel, ProductOption>(optionViewModel);
            option.ProductId = productId;
            var result = await _unitOfWork.ProductOptionRepository.UpdateAsync(option, id);
            var viewModel = ToModel<ProductOption, ProductOptionViewModel>(result);

            return HttpResult(viewModel);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOption(Guid id)
        {
            var result = await _unitOfWork.ProductOptionRepository.DeleteAsync(id);

            if (result > 0)
                return Ok();

            return NotFound();
        }

        private IHttpActionResult HttpResult<TModel>(TModel result)
        {
            if (result != null)
                return Ok(result);

            return NotFound();
        }

        private TModel ToModel<FModel, TModel>(FModel fromModel)
        {
            return _mapper.Map<FModel, TModel>(fromModel);
        }
    }
}
