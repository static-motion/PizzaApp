using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPriceFromShoppingCartPizzaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCartsPizzas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShoppingCartsPizzas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Calculated price including customizations");
        }
    }
}
