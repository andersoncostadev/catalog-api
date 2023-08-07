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

    public async Task<IEnumerable<Category>> GetCategoriesProducts()
    {
        return await Get().Include(p => p.Products).ToListAsync();
    }

    public async Task<PagedList<Category>> GetCategories(CategoriesParameters categoriesParameters)
    {
        return await PagedList<Category>.ToPagedList(Get().OrderBy(c => c.CategoryId),
            categoriesParameters.PageNumber,
            categoriesParameters.PageSize);
    }
}