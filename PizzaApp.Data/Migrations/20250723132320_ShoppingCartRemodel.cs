using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartRemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersPizzas_Pizzas_PizzaId",
                table: "OrdersPizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pizzas_Pizzas_BasePizzaId",
                table: "Pizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsPizzas_Pizzas_PizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsPizzas",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartsPizzas_PizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropIndex(
                name: "IX_Pizzas_BasePizzaId",
                table: "Pizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersPizzas",
                table: "OrdersPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrdersPizzas_PizzaId",
                table: "OrdersPizzas");

            migrationBuilder.DropColumn(
                name: "PizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "BasePizzaId",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "PizzaId",
                table: "OrdersPizzas");

            migrationBuilder.AlterTable(
                name: "ShoppingCartsPizzas",
                comment: "Items in user's shopping cart");

            migrationBuilder.AlterTable(
                name: "OrdersPizzas",
                comment: "A many-to-many mapping entity used to show which pizzas appear in which orders. ",
                oldComment: "A many-to-many mapping entity used to show which pizzas appear in which orders.");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 1,
                comment: "Quantity of this item in cart",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Primary Key unique identifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BasePizzaId",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign Key to base Pizza");

            migrationBuilder.AddColumn<string>(
                name: "PizzaComponentsJson",
                table: "ShoppingCartsPizzas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                comment: "JSON serialized pizza data");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShoppingCartsPizzas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Calculated price including customizations");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Orders. Shows which Order this pizza was used in.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Orders, part of composite Primary Key.");

            migrationBuilder.AddColumn<int>(
                name: "BasePizzaId",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign Key to Pizzas. This points to the original pizza the OrderPizza was based on.");

            migrationBuilder.AddColumn<int>(
                name: "DoughId",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Dough used for this specific order pizza");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Primary Key for OrderPizza. ");

            migrationBuilder.AddColumn<int>(
                name: "SauceId",
                table: "OrdersPizzas",
                type: "int",
                nullable: true,
                comment: "Sauce used for this specific order Pizza. Can be null.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsPizzas",
                table: "ShoppingCartsPizzas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersPizzas",
                table: "OrdersPizzas",
                columns: new[] { "OrderId", "BasePizzaId" });

            migrationBuilder.CreateTable(
                name: "OrderPizzaTopping",
                columns: table => new
                {
                    OrderPizzaId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to OrderPizzas, part of composite Primary Key."),
                    ToppingId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Toppings, part of composite Primary Key."),
                    PriceAtPurchase = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Price of the topping at the time of purchase"),
                    OrderPizzaBasePizzaId = table.Column<int>(type: "int", nullable: false),
                    OrderPizzaOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPizzaTopping", x => new { x.OrderPizzaId, x.ToppingId });
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_OrdersPizzas_OrderPizzaOrderId_OrderPizzaBasePizzaId",
                        columns: x => new { x.OrderPizzaOrderId, x.OrderPizzaBasePizzaId },
                        principalTable: "OrdersPizzas",
                        principalColumns: new[] { "OrderId", "BasePizzaId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Toppings for a specific pizza in an order");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_BasePizzaId",
                table: "ShoppingCartsPizzas",
                column: "BasePizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_ShoppingCartId",
                table: "ShoppingCartsPizzas",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_BasePizzaId",
                table: "OrdersPizzas",
                column: "BasePizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_DoughId",
                table: "OrdersPizzas",
                column: "DoughId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_SauceId",
                table: "OrdersPizzas",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaTopping_OrderPizzaOrderId_OrderPizzaBasePizzaId",
                table: "OrderPizzaTopping",
                columns: new[] { "OrderPizzaOrderId", "OrderPizzaBasePizzaId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaTopping_ToppingId",
                table: "OrderPizzaTopping",
                column: "ToppingId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersPizzas_Doughs_DoughId",
                table: "OrdersPizzas",
                column: "DoughId",
                principalTable: "Doughs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersPizzas_Pizzas_BasePizzaId",
                table: "OrdersPizzas",
                column: "BasePizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersPizzas_Sauces_SauceId",
                table: "OrdersPizzas",
                column: "SauceId",
                principalTable: "Sauces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsPizzas_Pizzas_BasePizzaId",
                table: "ShoppingCartsPizzas",
                column: "BasePizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersPizzas_Doughs_DoughId",
                table: "OrdersPizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersPizzas_Pizzas_BasePizzaId",
                table: "OrdersPizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersPizzas_Sauces_SauceId",
                table: "OrdersPizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartsPizzas_Pizzas_BasePizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropTable(
                name: "OrderPizzaTopping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartsPizzas",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartsPizzas_BasePizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartsPizzas_ShoppingCartId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersPizzas",
                table: "OrdersPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrdersPizzas_BasePizzaId",
                table: "OrdersPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrdersPizzas_DoughId",
                table: "OrdersPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrdersPizzas_SauceId",
                table: "OrdersPizzas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "BasePizzaId",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "PizzaComponentsJson",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCartsPizzas");

            migrationBuilder.DropColumn(
                name: "BasePizzaId",
                table: "OrdersPizzas");

            migrationBuilder.DropColumn(
                name: "DoughId",
                table: "OrdersPizzas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrdersPizzas");

            migrationBuilder.DropColumn(
                name: "SauceId",
                table: "OrdersPizzas");

            migrationBuilder.AlterTable(
                name: "ShoppingCartsPizzas",
                oldComment: "Items in user's shopping cart");

            migrationBuilder.AlterTable(
                name: "OrdersPizzas",
                comment: "A many-to-many mapping entity used to show which pizzas appear in which orders.",
                oldComment: "A many-to-many mapping entity used to show which pizzas appear in which orders. ");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1,
                oldComment: "Quantity of this item in cart");

            migrationBuilder.AddColumn<int>(
                name: "PizzaId",
                table: "ShoppingCartsPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BasePizzaId",
                table: "Pizzas",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Orders, part of composite Primary Key.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Orders. Shows which Order this pizza was used in.");

            migrationBuilder.AddColumn<int>(
                name: "PizzaId",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign Key to Pizzas, part of composite Primary Key.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartsPizzas",
                table: "ShoppingCartsPizzas",
                columns: new[] { "ShoppingCartId", "PizzaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersPizzas",
                table: "OrdersPizzas",
                columns: new[] { "OrderId", "PizzaId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_PizzaId",
                table: "ShoppingCartsPizzas",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_BasePizzaId",
                table: "Pizzas",
                column: "BasePizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_PizzaId",
                table: "OrdersPizzas",
                column: "PizzaId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersPizzas_Pizzas_PizzaId",
                table: "OrdersPizzas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pizzas_Pizzas_BasePizzaId",
                table: "Pizzas",
                column: "BasePizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartsPizzas_Pizzas_PizzaId",
                table: "ShoppingCartsPizzas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
