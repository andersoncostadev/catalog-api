using APICatalogo.Domain;

namespace APICatalogo.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesProducts();
}