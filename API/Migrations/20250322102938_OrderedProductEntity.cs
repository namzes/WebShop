using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.API.Migrations
{
    /// <inheritdoc />
    public partial class OrderedProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_Orders_OrderId",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_OrderId",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartProducts");

            migrationBuilder.CreateTable(
                name: "OrderedProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedProduct_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderedProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProduct_OrderId",
                table: "OrderedProduct",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProduct_ProductId",
                table: "OrderedProduct",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedProduct");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CartProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_OrderId",
                table: "CartProducts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_Orders_OrderId",
                table: "CartProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
