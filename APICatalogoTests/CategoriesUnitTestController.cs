using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APICatalogoTests;

public class CategoriesUnitTestController
{
    private IMapper mapper;
    private IUnitOfWork unitOfWork;
    
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString = "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=admin123;";
    
    static CategoriesUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public CategoriesUnitTestController()
    {
       var config = new MapperConfiguration(cfg =>
       {
           cfg.AddProfile(new MappingProfile());
       }); 
       mapper = config.CreateMapper();
       var context = new AppDbContext(dbContextOptions);

       // DbUnitTestsMockInitializer db = new DbUnitTestsMockInitializer();
       // db.Seed(context);

       unitOfWork = new UnitOfWork(context);
    }
    
    [Fact]
    public async Task GetCategorieById_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriesController(unitOfWork, mapper);
        const int catId = 2;

        // Act
        var data = await controller.Get(catId);

        // Assert
        var result = Assert.IsType<OkObjectResult>(data.Result);
        var categoryDto = Assert.IsType<CategoryDTO>(result.Value);
        Assert.Equal(catId, categoryDto.CategoryId);
    }
    
    [Fact]
    public async Task GetCategoryById_Return_NotFound()
    {
        // Arrange
        var controller = new CategoriesController(unitOfWork, mapper);
        const int nonExistentCatId = 999; 

        // Act
        var data = await controller.Get(nonExistentCatId);

        // Assert
        var result = Assert.IsType<NotFoundObjectResult>(data.Result);
        Assert.Equal("Categoria não encontrada!", result.Value);
    }
    
    [Fact]
    public async Task Post_Category_AddValidData_Return_CreatedResult()
    {
        // Arrange
        var controller = new CategoriesController(unitOfWork, mapper);
        var catDto = new CategoryDTO() { Name = "Categoria Teste Unitário", ImageUrl = "testeunitario.jpg"};

        // Act
        var data = await controller.Post(catDto);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(data);
    }

    [Fact]
    public async Task Put_Category_Update_ValidData_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriesController(unitOfWork, mapper);
        const int catId = 5;
        var catDto = new CategoryDTO();
        catDto.CategoryId = catId;
        catDto.Name = "Categoria Teste Unitário UPDATE";
        catDto.ImageUrl = "nova-url-da-imagem";

        // Act
        var updatedData = await controller.Put(catId, catDto);

        // Assert
        var result = Assert.IsType<OkObjectResult>(updatedData);
        var updatedCategoryDto = Assert.IsType<CategoryDTO>(result.Value);
        
        Assert.Equal(catDto.CategoryId, updatedCategoryDto.CategoryId);
        Assert.Equal(catDto.Name, updatedCategoryDto.Name);
        Assert.Equal(catDto.ImageUrl, updatedCategoryDto.ImageUrl);
    }

    [Fact]
    public async Task Delete_Category_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriesController(unitOfWork, mapper);
        const int catId = 6;

        // Act
        var data = await controller.Delete(catId);

        // Assert
        var result = Assert.IsType<OkObjectResult>(data.Result);
        var deletedCategory = Assert.IsType<CategoryDTO>(result.Value);
        
        Assert.Equal(catId, deletedCategory.CategoryId);
    }
    
}
    
