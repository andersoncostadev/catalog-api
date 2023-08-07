using APICatalogo.Domain;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("lowestprice")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsPrice()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsPrice();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);
            
            return productsDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _unitOfWork.ProductRepository.GetProducts(productsParameters);
            
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };
            
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            
            var productsDto = _mapper.Map<List<ProductDTO>>(products);

            if (products == null || products.Count == 0) return NotFound("Produtos não encontrado");

            return productsDto;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado!");
            
            var productDto = _mapper.Map<Product>(product);

            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (product is null) return BadRequest();

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.Commit();

            var productDtoCreated = _mapper.Map<ProductDTO>(product);

            return new CreatedAtRouteResult("ObterProduto", new {id = productDto.ProductId }, productDtoCreated);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductDTO productDto) 
        {
            if(id != productDto.ProductId) return BadRequest("Dados inválidos!");
            
            var product = _mapper.Map<Product>(productDto);

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Commit();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado...");

            _unitOfWork.ProductRepository.Delete(product);
             await _unitOfWork.Commit();
            
            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }
    }
}
