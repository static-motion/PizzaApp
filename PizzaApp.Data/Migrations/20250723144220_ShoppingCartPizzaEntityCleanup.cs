using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartPizzaEntityCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ShoppingCartsPizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to User. Indicates whose shopping cart this pizza is in.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ShoppingCartsPizzas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to User. Indicates whose shopping cart this pizza is in.");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
