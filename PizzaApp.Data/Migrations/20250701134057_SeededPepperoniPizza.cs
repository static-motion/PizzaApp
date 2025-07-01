using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededPepperoniPizza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desicription",
                table: "Toppings",
                newName: "Description");

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "CreatorUserId", "DoughId", "ImageUrl", "Name", "SauceId" },
                values: new object[] { 1, new Guid("7bc9cf3b-7464-4b4a-ea3b-08ddb8a10943"), 1, null, "Classic Pepperoni", 1 });

            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "Id", "Description", "Name", "Price", "ToppingCategoryId" },
                values: new object[,]
                {
                    { 1, "A spicy, cured Italian-American sausage with a bold, savory flavor and a slightly crispy texture when baked.", "Pepperoni", 1m, 1 },
                    { 2, "Smoky, crispy and irresistibly delicious, bacon makes everything better - especially pizza!", "Bacon", 1m, 1 },
                    { 3, "Spicy, smoky Spanish sausage that kicks pizza up a notch - a flavor fiesta in every bite.", "Chorizo", 1m, 1 },
                    { 4, "Creamy, melty, stretchy perfection. Pizza without mozzarella is just sad bread.", "Mozzarella", 1m, 2 },
                    { 5, "Sharp, tangy, and gloriously gooey. Cheddar brings a bold twist to pizza that basic cheeses can't match.", "Cheddar", 1m, 2 },
                    { 6, "Salty, nutty, and irresistibly savory. Parmesan is the finishing touch that elevates pizza from good to gourmet.", "Parmesan", 1m, 2 },
                    { 7, "Velvety, indulgent, and irresistibly smooth. Philadelphia cheese turns pizza into a decadent delight.", "Philadelphia", 1m, 2 }
                });

            migrationBuilder.InsertData(
                table: "PizzasToppings",
                columns: new[] { "PizzaId", "ToppingId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PizzasToppings",
                keyColumns: new[] { "PizzaId", "ToppingId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PizzasToppings",
                keyColumns: new[] { "PizzaId", "ToppingId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Toppings",
                newName: "Desicription");
        }
    }
}
