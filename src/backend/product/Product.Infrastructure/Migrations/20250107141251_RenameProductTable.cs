using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                schema: "dbo",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                schema: "dbo",
                newName: "Products",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Price",
                schema: "dbo",
                table: "Products",
                newName: "IX_Products_Price");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Name",
                schema: "dbo",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CreatedAt",
                schema: "dbo",
                table: "Products",
                newName: "IX_Products_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "dbo",
                table: "Products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "dbo",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "dbo",
                newName: "Product",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Price",
                schema: "dbo",
                table: "Product",
                newName: "IX_Product_Price");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                schema: "dbo",
                table: "Product",
                newName: "IX_Product_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatedAt",
                schema: "dbo",
                table: "Product",
                newName: "IX_Product_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                schema: "dbo",
                table: "Product",
                column: "Id");
        }
    }
}
