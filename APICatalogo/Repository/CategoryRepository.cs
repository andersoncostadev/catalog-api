using APICatalogo.Context;
using APICatalogo.Domain;
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
}