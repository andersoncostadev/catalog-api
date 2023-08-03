using APICatalogo.Domain;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("lowestprice")]
        public ActionResult<IEnumerable<Product>> GetProductsPrice()
        {
            return _unitOfWork.ProductRepository.GetProductsPrice().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _unitOfWork.ProductRepository.Get().ToList();

            if (products == null || products.Count == 0) return NotFound("Produtos não encontrado");

            return products;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado!");

            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null) return BadRequest();

            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Commit();

           return new CreatedAtRouteResult("ObterProduto", new {id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {
            if(id != product.ProductId) return BadRequest();

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado...");

            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();

            return Ok(product);
        }
    }
}
