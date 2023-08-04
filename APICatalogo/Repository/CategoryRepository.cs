using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class CategoryRepository: Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Category> GetCategoriesProducts()
    {
        return Get().Include(p => p.Products);
    }

    public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
    {
        return PagedList<Category>.ToPagedList(Get().OrderBy(c => c.CategoryId),
            categoriesParameters.PageNumber,
            categoriesParameters.PageSize);
    }
}