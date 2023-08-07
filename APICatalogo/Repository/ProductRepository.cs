using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsPrice()
    {
        return await Get().OrderBy(c => c.Price).ToListAsync();
    }

    // public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
    // {
    //     return Get().OrderBy(n => n.Name)
    //         .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
    //         .Take(productsParameters.PageSize)
    //         .ToList();
    // }
    
    public async Task<PagedList<Product>> GetProducts(ProductsParameters productsParameters)
    {
        return await PagedList<Product>.ToPagedList(Get().OrderBy(p => p.ProductId),
            productsParameters.PageNumber,
            productsParameters.PageSize);
    }
}