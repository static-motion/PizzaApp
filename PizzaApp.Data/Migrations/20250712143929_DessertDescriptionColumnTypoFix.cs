using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DessertDescriptionColumnTypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripion",
                table: "Desserts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Drinks",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: false,
                defaultValue: "",
                comment: "Short description of the drink.",
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldMaxLength: 75,
                oldNullable: true,
                oldComment: "Short description of the drink.");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Short description of the dessert.");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Rich cheesecake with blueberry jam! We could eat this all day!");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Home baked with our special dough recipe and luxurious dark chocolate! Yum!");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Home is where the heart is. Or where the best apple pie is. We're still not sure...");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Vanilla Ice Cream. Strawberry syrup. Need we say more?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Desserts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Drinks",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true,
                comment: "Short description of the drink.",
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldMaxLength: 75,
                oldComment: "Short description of the drink.");

            migrationBuilder.AddColumn<string>(
                name: "Descripion",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                comment: "Short description of the dessert.");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripion",
                value: "Rich cheesecake with blueberry jam! We could eat this all day!");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripion",
                value: "Home baked with our special dough recipe and luxurious dark chocolate! Yum!");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripion",
                value: "Home is where the heart is. Or where the best apple pie is. We're still not sure...");

            migrationBuilder.UpdateData(
                table: "Desserts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripion",
                value: "Vanilla Ice Cream. Strawberry syrup. Need we say more?");
        }
    }
}
