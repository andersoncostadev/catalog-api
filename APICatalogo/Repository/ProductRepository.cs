using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Pagination;

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

    // public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
    // {
    //     return Get().OrderBy(n => n.Name)
    //         .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
    //         .Take(productsParameters.PageSize)
    //         .ToList();
    // }
    
    public PagedList<Product> GetProducts(ProductsParameters productsParameters)
    {
        return PagedList<Product>.ToPagedList(Get().OrderBy(p => p.ProductId),
            productsParameters.PageNumber,
            productsParameters.PageSize);
    }
}