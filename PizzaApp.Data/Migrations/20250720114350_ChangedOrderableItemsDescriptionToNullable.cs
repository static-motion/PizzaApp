using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOrderableItemsDescriptionToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pizzas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "Short pizza description",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "Short pizza description");

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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                comment: "Short description of the dessert.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "Short description of the dessert.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pizzas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Short pizza description",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Short pizza description");

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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Short description of the dessert.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true,
                oldComment: "Short description of the dessert.");
        }
    }
}
