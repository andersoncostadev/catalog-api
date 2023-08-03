using APICatalogo.Domain;

namespace APICatalogo.Repository;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsPrice();
}