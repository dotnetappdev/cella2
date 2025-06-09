using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cella.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class newfeld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Customer",
                table: "SalesOrders",
                newName: "CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesOrderId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SalesOrderId",
                table: "Customers",
                column: "SalesOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SalesOrders_SalesOrderId",
                table: "Customers",
                column: "SalesOrderId",
                principalTable: "SalesOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SalesOrders_SalesOrderId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SalesOrderId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "SalesOrderId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "SalesOrders",
                newName: "Customer");
        }
    }
}
