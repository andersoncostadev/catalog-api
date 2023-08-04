using APICatalogo.Domain;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesProducts();
    
    PagedList<Category> GetCategories(CategoriesParameters categoriesParameters);
}