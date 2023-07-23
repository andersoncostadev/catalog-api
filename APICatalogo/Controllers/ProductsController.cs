using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context.Products.AsNoTracking().ToList();

            if (products == null || products.Count == 0) return NotFound("Produtos não encontrado");

            return products;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);

            if (product is null) return NotFound("Produto não encontrado!");

            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null) return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();

           return new CreatedAtRouteResult("ObterProduto", new {id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {
            if(id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
           // var product = _context.Products.Find(id); // vantagem busca na memória

            if (product is null) return NotFound("Produto não encontrado...");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }
}
