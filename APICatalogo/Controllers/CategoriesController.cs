using APICatalogo.Domain;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> ProductsCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesProducts();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories);
            
            return categoriesDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategories(categoriesParameters);

            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDto =  _mapper.Map<List<CategoryDTO>>(categories);

            if (categories == null || categories.Count == 0) return NotFound("Categorias não encontradas");

            return categoriesDto;
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);

            if(category == null) return NotFound("Categoria não encontrada!");
            
            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoryDTO categoryDto) 
        {
            var category = _mapper.Map<Category>(categoryDto);
            
            if (category is null) return BadRequest("Dados inválidos!");

            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.Commit();
            
            var categoryDtoCreated = _mapper.Map<CategoryDTO>(category);

            return new CreatedAtRouteResult("ObterCategoria", new {id = categoryDto.CategoryId}, categoryDtoCreated);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId) return BadRequest("Dados inválidos!"); 
            
            var category = _mapper.Map<Category>(categoryDto);
            
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.Commit();

            var updatedCategoryDto = _mapper.Map<CategoryDTO>(category);

            return Ok(updatedCategoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);

            if (category == null) return NotFound("Categoria não encontrada");

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.Commit();
            
            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return Ok(categoryDto);
        }
    }
}
