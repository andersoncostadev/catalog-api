using APICatalogo.Context;

namespace APICatalogo.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ProductRepository _productRepository;
    private CategoryRepository _categoryRepository;
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository = _productRepository ?? new ProductRepository(_context);
        }
    }

    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);

    public async Task Commit()
    {
       await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}