using APICatalogo.Context;
using APICatalogo.Domain;

namespace APICatalogo.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Product> GetProductsPrice()
    {
        return Get().OrderBy(c => c.Price).ToList();
    }
}