namespace APICatalogo.Repository;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    
    ICategoryRepository CategoryRepository { get; }

    void Commit();
}