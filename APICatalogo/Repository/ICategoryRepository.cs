using APICatalogo.Domain;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface ICategoryRepository : IRepository<Category>
{
   Task<IEnumerable<Category>> GetCategoriesProducts();
    
    Task<PagedList<Category>> GetCategories(CategoriesParameters categoriesParameters);
}