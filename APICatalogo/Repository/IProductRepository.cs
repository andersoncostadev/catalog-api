using APICatalogo.Domain;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsPrice();
    
    Task<PagedList<Product>> GetProducts(ProductsParameters productsParameters);
}