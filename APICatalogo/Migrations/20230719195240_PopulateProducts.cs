using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegisterDate,CategoryId)" +
                "Values('Coca-Cola Diet','Refigerante de Cola 350ml',5.45,'cocacola.jpg',50,now(),1)");

            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegisterDate,CategoryId)" +
                "Values('Lanche de Atum','Lanche de Atum com maionese',8.50,'atum.jpg',10,now(),2)");

            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegisterDate,CategoryId)" +
                "Values('Pudim 100g','Pudim de leite condesado 100g',6.75,'pudim.jpg',20,now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Products");
        }
    }
}
