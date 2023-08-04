using APICatalogo.Domain;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsPrice();
    
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
}