using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededVegetablesToppings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "Id", "Description", "IsDeleted", "Name", "Price", "ToppingCategoryId" },
                values: new object[,]
                {
                    { 8, "Nature’s way of saying, ‘Yeah, this pizza needed more color.’", false, "Bell Peppers", 1m, 3 },
                    { 9, "The pizza topping that makes vegetarians and carnivores high-five!", false, "Mushrooms", 1m, 3 },
                    { 10, "Pizza’s way of keeping first dates interesting.", false, "Onions", 1m, 3 },
                    { 11, "Tiny, salty, and judging you for picking them off.", false, "Olives", 1m, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
