using APICatalogo.Domain;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> ProductsCategories()
        {
            return _unitOfWork.CategoryRepository.GetCategoriesProducts().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            return _unitOfWork.CategoryRepository.Get().ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Category> Get(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);

            if(category == null) return NotFound("Categoria não encontrada!");

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category) 
        {
            if (category == null) return BadRequest("Dados inválidos!");

            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new {id = category.CategoryId}, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId) return BadRequest("Dados inválidos!"); 
            
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Commit();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);

            if (category == null) return NotFound("Categoria não encontrada");

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            return Ok(category);
        }
    }
}
