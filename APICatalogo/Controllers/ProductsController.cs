using APICatalogo.Domain;
using APICatalogo.DTOs;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<ProductDTO>> GetProductsPrice()
        {
            var products = _unitOfWork.ProductRepository.GetProductsPrice().ToList();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);
            
            return productsDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            var products = _unitOfWork.ProductRepository.Get().ToList();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);

            if (products == null || products.Count == 0) return NotFound("Produtos não encontrado");

            return productsDto;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado!");
            
            var productDto = _mapper.Map<Product>(product);

            return Ok(productDto);
        }

        [HttpPost]
        public ActionResult Post(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (product is null) return BadRequest();

            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Commit();

            var productDtoCreated = _mapper.Map<ProductDTO>(product);

            return new CreatedAtRouteResult("ObterProduto", new {id = productDto.ProductId }, productDtoCreated);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProductDTO productDto) 
        {
            if(id != productDto.ProductId) return BadRequest("Dados inválidos!");
            
            var product = _mapper.Map<Product>(productDto);

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado...");

            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();
            
            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }
    }
}
