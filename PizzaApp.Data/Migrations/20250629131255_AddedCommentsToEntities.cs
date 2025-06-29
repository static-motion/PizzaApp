using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommentsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzaTopping_Pizzas_PizzaId",
                table: "PizzaTopping");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzaTopping_Toppings_ToppingId",
                table: "PizzaTopping");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_ToppingCategories_ToppingTypeId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ToppingTypeId",
                table: "Toppings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PizzaTopping",
                table: "PizzaTopping");

            migrationBuilder.DropColumn(
                name: "ToppingTypeId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "PricePerItemAtPurchase",
                table: "PizzaTopping");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PizzaTopping");

            migrationBuilder.RenameTable(
                name: "PizzaTopping",
                newName: "PizzasToppings");

            migrationBuilder.RenameIndex(
                name: "IX_PizzaTopping_PizzaId",
                table: "PizzasToppings",
                newName: "IX_PizzasToppings_PizzaId");

            migrationBuilder.AlterTable(
                name: "UsersPizzas",
                comment: "A many-to-many mapping entity between User and Pizza, showing pizza entities which have been marked as favorite by users");

            migrationBuilder.AlterTable(
                name: "Toppings",
                comment: "All the toppings offered.");

            migrationBuilder.AlterTable(
                name: "ToppingCategories",
                comment: "The topping categories offered by the pizza app (meats, veggies etc.)");

            migrationBuilder.AlterTable(
                name: "Sauces",
                comment: "All the sauces offered.");

            migrationBuilder.AlterTable(
                name: "Pizzas",
                comment: "All pizzas offered - both admin and user created.");

            migrationBuilder.AlterTable(
                name: "OrdersPizzas",
                comment: "A many-to-many mapping entity used to show which pizzas appear in which orders.");

            migrationBuilder.AlterTable(
                name: "OrdersDrinks",
                comment: "A many-to-many mapping entity used to show which drinks appear in which orders.");

            migrationBuilder.AlterTable(
                name: "OrdersDesserts",
                comment: "A many-to-many mapping entity used to show which desserts appear in which orders.");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "All the users' orders in the database.");

            migrationBuilder.AlterTable(
                name: "Drinks",
                comment: "All the drinks offered.");

            migrationBuilder.AlterTable(
                name: "Doughs",
                comment: "All the dough types used for making pizzas.");

            migrationBuilder.AlterTable(
                name: "Desserts",
                comment: "All the desserts offered.");

            migrationBuilder.AlterTable(
                name: "AspNetUsers",
                comment: "The general public website user. This entity has addresses, created pizzas, favorited pizzas and order associated with it.");

            migrationBuilder.AlterTable(
                name: "Addresses",
                comment: "All the addresses as created by the Users.");

            migrationBuilder.AlterTable(
                name: "PizzasToppings",
                comment: "A many-to-many mapping entity between Pizza and Toppings, used to show which toppings are contained in which pizzas.");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "UsersPizzas",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Pizzas",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UsersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Users",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Toppings",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                comment: "Current price of the pizza topping",
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldComment: "Topping price");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Toppings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the pizza topping",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Topping name");

            migrationBuilder.AlterColumn<string>(
                name: "Desicription",
                table: "Toppings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "A short description of the pizza topping.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "Topping description");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Toppings",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Unique identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ToppingCategoryId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign key to topping categories, shows which category the topping belongs to (meats, veggies etc.)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Sauces",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Sauce type (tomato, pesto etc.)",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Sauce type");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sauces",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                comment: "Current sauce price",
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldComment: "Sauce Price");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sauces",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "Short sauce description",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "Sauce description");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sauces",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Unique identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SauceId",
                table: "Pizzas",
                type: "int",
                nullable: true,
                comment: "The sauce used on the pizza. Can be null.",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pizzas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the pizza as given by its creator (User)",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Pizzas",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "URL of the image of the pizza.",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoughId",
                table: "Pizzas",
                type: "int",
                nullable: false,
                comment: "The type of dough the pizza is made with.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorUserId",
                table: "Pizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to User who created the pizza.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Pizzas",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier.",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                comment: "Quantity of pizzas in the order.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersPizzas",
                type: "decimal(8,2)",
                nullable: false,
                comment: "Price of the pizza at the time of purchase, used for total price calculations.",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Pizzas, part of composite Primary Key.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Orders, part of composite Primary Key.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersDrinks",
                type: "int",
                nullable: false,
                comment: "Ordered drink quantity",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersDrinks",
                type: "decimal(8,2)",
                nullable: false,
                comment: "Price of the drink at the time of purchase, used for total price calculations.",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkId",
                table: "OrdersDrinks",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Drinks, part of composite Primary Key.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersDrinks",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Orders, part of composite Primary Key.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersDesserts",
                type: "int",
                nullable: false,
                comment: "Dessert quantity ordered.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersDesserts",
                type: "decimal(8,2)",
                nullable: false,
                comment: "Price of the pizza at the time of purchase, used for total price calculations.",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DessertId",
                table: "OrdersDesserts",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Desserts, part of composite Primary Key.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersDesserts",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Orders, part of composite Primary Key.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Users - the user who made the order.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Price of the order.",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Current status of the order.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Addresses - location where the order was supposed to be delivered.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Primary Key unique identifier",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date and time at which the order was created.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Drinks",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                comment: "Current Price of the drink.",
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drinks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the drink.",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Drinks",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "URL for the image of the drink.",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

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
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Drinks",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Desserts",
                type: "decimal(8,2)",
                nullable: false,
                comment: "Current price of the dessert.",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Desserts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the dessert",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Desserts",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                comment: "URL for the image of the dessert.",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripion",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                comment: "Short description of the dessert.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Desserts",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier.",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign Key to Users table - User who is associated with this address.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "bit",
                nullable: false,
                comment: "Shows if the address has been deleted.",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "The city where the address is located at.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                comment: "Second address line. Can be null.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "First address line.",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Addresses",
                type: "int",
                nullable: false,
                comment: "Primary Key unique identifier.",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "PizzasToppings",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Pizzas, part of composite Primary Key.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ToppingId",
                table: "PizzasToppings",
                type: "int",
                nullable: false,
                comment: "Foreign Key to Toppings, part of composite Primary Key.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PizzasToppings",
                table: "PizzasToppings",
                columns: new[] { "ToppingId", "PizzaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ToppingCategoryId",
                table: "Toppings",
                column: "ToppingCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PizzasToppings_Pizzas_PizzaId",
                table: "PizzasToppings",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PizzasToppings_Toppings_ToppingId",
                table: "PizzasToppings",
                column: "ToppingId",
                principalTable: "Toppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_ToppingCategories_ToppingCategoryId",
                table: "Toppings",
                column: "ToppingCategoryId",
                principalTable: "ToppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzasToppings_Pizzas_PizzaId",
                table: "PizzasToppings");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzasToppings_Toppings_ToppingId",
                table: "PizzasToppings");

            migrationBuilder.DropForeignKey(
                name: "FK_Toppings_ToppingCategories_ToppingCategoryId",
                table: "Toppings");

            migrationBuilder.DropIndex(
                name: "IX_Toppings_ToppingCategoryId",
                table: "Toppings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PizzasToppings",
                table: "PizzasToppings");

            migrationBuilder.DropColumn(
                name: "ToppingCategoryId",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "PizzasToppings",
                newName: "PizzaTopping");

            migrationBuilder.RenameIndex(
                name: "IX_PizzasToppings_PizzaId",
                table: "PizzaTopping",
                newName: "IX_PizzaTopping_PizzaId");

            migrationBuilder.AlterTable(
                name: "UsersPizzas",
                oldComment: "A many-to-many mapping entity between User and Pizza, showing pizza entities which have been marked as favorite by users");

            migrationBuilder.AlterTable(
                name: "Toppings",
                oldComment: "All the toppings offered.");

            migrationBuilder.AlterTable(
                name: "ToppingCategories",
                oldComment: "The topping categories offered by the pizza app (meats, veggies etc.)");

            migrationBuilder.AlterTable(
                name: "Sauces",
                oldComment: "All the sauces offered.");

            migrationBuilder.AlterTable(
                name: "Pizzas",
                oldComment: "All pizzas offered - both admin and user created.");

            migrationBuilder.AlterTable(
                name: "OrdersPizzas",
                oldComment: "A many-to-many mapping entity used to show which pizzas appear in which orders.");

            migrationBuilder.AlterTable(
                name: "OrdersDrinks",
                oldComment: "A many-to-many mapping entity used to show which drinks appear in which orders.");

            migrationBuilder.AlterTable(
                name: "OrdersDesserts",
                oldComment: "A many-to-many mapping entity used to show which desserts appear in which orders.");

            migrationBuilder.AlterTable(
                name: "Orders",
                oldComment: "All the users' orders in the database.");

            migrationBuilder.AlterTable(
                name: "Drinks",
                oldComment: "All the drinks offered.");

            migrationBuilder.AlterTable(
                name: "Doughs",
                oldComment: "All the dough types used for making pizzas.");

            migrationBuilder.AlterTable(
                name: "Desserts",
                oldComment: "All the desserts offered.");

            migrationBuilder.AlterTable(
                name: "AspNetUsers",
                oldComment: "The general public website user. This entity has addresses, created pizzas, favorited pizzas and order associated with it.");

            migrationBuilder.AlterTable(
                name: "Addresses",
                oldComment: "All the addresses as created by the Users.");

            migrationBuilder.AlterTable(
                name: "PizzaTopping",
                oldComment: "A many-to-many mapping entity between Pizza and Toppings, used to show which toppings are contained in which pizzas.");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "UsersPizzas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Pizzas");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UsersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Toppings",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                comment: "Topping price",
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldComment: "Current price of the pizza topping");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Toppings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Topping name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the pizza topping");

            migrationBuilder.AlterColumn<string>(
                name: "Desicription",
                table: "Toppings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "Topping description",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "A short description of the pizza topping.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Toppings",
                type: "int",
                nullable: false,
                comment: "Unique identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ToppingTypeId",
                table: "Toppings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign key to topping types");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Sauces",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Sauce type",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Sauce type (tomato, pesto etc.)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sauces",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                comment: "Sauce Price",
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldComment: "Current sauce price");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sauces",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "Sauce description",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "Short sauce description");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sauces",
                type: "int",
                nullable: false,
                comment: "Unique identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SauceId",
                table: "Pizzas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "The sauce used on the pizza. Can be null.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pizzas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the pizza as given by its creator (User)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Pizzas",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "URL of the image of the pizza.");

            migrationBuilder.AlterColumn<int>(
                name: "DoughId",
                table: "Pizzas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The type of dough the pizza is made with.");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorUserId",
                table: "Pizzas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to User who created the pizza.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Pizzas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier.")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Quantity of pizzas in the order.");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersPizzas",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldComment: "Price of the pizza at the time of purchase, used for total price calculations.");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "OrdersPizzas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Pizzas, part of composite Primary Key.");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersPizzas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Orders, part of composite Primary Key.");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersDrinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ordered drink quantity");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersDrinks",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldComment: "Price of the drink at the time of purchase, used for total price calculations.");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkId",
                table: "OrdersDrinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Drinks, part of composite Primary Key.");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersDrinks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Orders, part of composite Primary Key.");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrdersDesserts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Dessert quantity ordered.");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "OrdersDesserts",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldComment: "Price of the pizza at the time of purchase, used for total price calculations.");

            migrationBuilder.AlterColumn<int>(
                name: "DessertId",
                table: "OrdersDesserts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Desserts, part of composite Primary Key.");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrdersDesserts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Orders, part of composite Primary Key.");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Users - the user who made the order.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Price of the order.");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Current status of the order.");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Addresses - location where the order was supposed to be delivered.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Primary Key unique identifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Drinks",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldComment: "Current Price of the drink.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drinks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the drink.");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Drinks",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "URL for the image of the drink.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Drinks",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldMaxLength: 75,
                oldNullable: true,
                oldComment: "Short description of the drink.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Drinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Desserts",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldComment: "Current price of the dessert.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Desserts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the dessert");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Desserts",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true,
                oldComment: "URL for the image of the dessert.");

            migrationBuilder.AlterColumn<string>(
                name: "Descripion",
                table: "Desserts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true,
                oldComment: "Short description of the dessert.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Desserts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier.")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign Key to Users table - User who is associated with this address.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Shows if the address has been deleted.");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "The city where the address is located at.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true,
                oldComment: "Second address line. Can be null.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                table: "Addresses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "First address line.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Addresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary Key unique identifier.")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PizzaId",
                table: "PizzaTopping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Pizzas, part of composite Primary Key.");

            migrationBuilder.AlterColumn<int>(
                name: "ToppingId",
                table: "PizzaTopping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Foreign Key to Toppings, part of composite Primary Key.");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerItemAtPurchase",
                table: "PizzaTopping",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PizzaTopping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PizzaTopping",
                table: "PizzaTopping",
                columns: new[] { "ToppingId", "PizzaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ToppingTypeId",
                table: "Toppings",
                column: "ToppingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaTopping_Pizzas_PizzaId",
                table: "PizzaTopping",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaTopping_Toppings_ToppingId",
                table: "PizzaTopping",
                column: "ToppingId",
                principalTable: "Toppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Toppings_ToppingCategories_ToppingTypeId",
                table: "Toppings",
                column: "ToppingTypeId",
                principalTable: "ToppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
