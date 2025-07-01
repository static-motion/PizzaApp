using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPizzaDescriptionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pizzas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Short pizza description");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Tomato sauce, mozzarella and pepperoni on white dough. Simple. Classic. Timeless.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pizzas");
        }
    }
}
