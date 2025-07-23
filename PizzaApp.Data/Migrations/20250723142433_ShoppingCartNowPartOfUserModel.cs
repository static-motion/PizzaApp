using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartNowPartOfUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ShoppingCarts_ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsDesserts_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsDesserts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsDrinks_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsPizzas_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartsPizzas_ShoppingCartId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsDrinks",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsDesserts",
                table: "ShoppingCartsDesserts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartsDesserts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ShoppingCartsPizzas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ShoppingCartsDrinks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ShoppingCartsDesserts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsDrinks",
                table: "ShoppingCartsDrinks",
                columns: new[] { "UserId", "DrinkId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsDesserts",
                table: "ShoppingCartsDesserts",
                columns: new[] { "UserId", "DessertId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_UserId",
                table: "ShoppingCartsPizzas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsDesserts_AspNetUsers_UserId",
                table: "ShoppingCartsDesserts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsDrinks_AspNetUsers_UserId",
                table: "ShoppingCartsDrinks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsPizzas_AspNetUsers_UserId",
                table: "ShoppingCartsPizzas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsDesserts_AspNetUsers_UserId",
                table: "ShoppingCartsDesserts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsDrinks_AspNetUsers_UserId",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsPizzas_AspNetUsers_UserId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartsPizzas_UserId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsDrinks",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsDesserts",
                table: "ShoppingCartsDesserts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartsDrinks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartsDesserts");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartsDrinks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartsDesserts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsDrinks",
                table: "ShoppingCartsDrinks",
                columns: new[] { "ShoppingCartId", "DrinkId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsDesserts",
                table: "ShoppingCartsDesserts",
                columns: new[] { "ShoppingCartId", "DessertId" });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_ShoppingCartId",
                table: "ShoppingCartsPizzas",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShoppingCartId",
                table: "AspNetUsers",
                column: "ShoppingCartId",
                unique: true,
                filter: "[ShoppingCartId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ShoppingCarts_ShoppingCartId",
                table: "AspNetUsers",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsDesserts_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsDesserts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsDrinks_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsDrinks",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsPizzas_ShoppingCarts_ShoppingCartId",
                table: "ShoppingCartsPizzas",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
