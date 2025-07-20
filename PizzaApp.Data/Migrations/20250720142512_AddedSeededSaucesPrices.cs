using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeededSaucesPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 1m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 1m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 1m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 1m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Sauces",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 0m);
        }
    }
}
