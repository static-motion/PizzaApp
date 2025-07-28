using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RECREATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                },
                comment: "The general public website user. This entity has addresses, created pizzas, favorited pizzas and order associated with it.");

            migrationBuilder.CreateTable(
                name: "Desserts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the dessert"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "Short description of the dessert."),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Current price of the dessert."),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, comment: "URL for the image of the dessert."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desserts", x => x.Id);
                },
                comment: "All the desserts offered.");

            migrationBuilder.CreateTable(
                name: "Doughs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Dough type"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Dough description"),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "Dough price"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doughs", x => x.Id);
                },
                comment: "All the dough types used for making pizzas.");

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the drink."),
                    Description = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true, comment: "Short description of the drink."),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "Current Price of the drink."),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, comment: "URL for the image of the drink."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                },
                comment: "All the drinks offered.");

            migrationBuilder.CreateTable(
                name: "Sauces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Sauce type (tomato, pesto etc.)"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Short sauce description"),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "Current sauce price"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sauces", x => x.Id);
                },
                comment: "All the sauces offered.");

            migrationBuilder.CreateTable(
                name: "ToppingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Topping type name"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToppingCategories", x => x.Id);
                },
                comment: "The topping categories offered by the pizza app (meats, veggies etc.)");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The city where the address is located at."),
                    AddressLine1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "First address line."),
                    AddressLine2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "Second address line. Can be null."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Users table - User who is associated with this address."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the address has been soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "All the addresses as created by the Users.");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartsDesserts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DessertId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartsDesserts", x => new { x.UserId, x.DessertId });
                    table.ForeignKey(
                        name: "FK_ShoppingCartsDesserts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartsDesserts_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartsDrinks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartsDrinks", x => new { x.UserId, x.DrinkId });
                    table.ForeignKey(
                        name: "FK_ShoppingCartsDrinks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartsDrinks_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the pizza as given by its creator (User)"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Short pizza description"),
                    DoughId = table.Column<int>(type: "int", nullable: false, comment: "The type of dough the pizza is made with."),
                    SauceId = table.Column<int>(type: "int", nullable: true, comment: "The sauce used on the pizza. Can be null."),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, comment: "URL of the image of the pizza."),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to User who created the pizza."),
                    PizzaType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the pizza has been soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pizzas_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pizzas_Doughs_DoughId",
                        column: x => x.DoughId,
                        principalTable: "Doughs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pizzas_Sauces_SauceId",
                        column: x => x.SauceId,
                        principalTable: "Sauces",
                        principalColumn: "Id");
                },
                comment: "All pizzas offered - both admin and user created.");

            migrationBuilder.CreateTable(
                name: "Toppings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToppingCategoryId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to topping categories, shows which category the topping belongs to (meats, veggies etc.)"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the pizza topping"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "A short description of the pizza topping."),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "Current price of the pizza topping"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Shows if the entity is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toppings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toppings_ToppingCategories_ToppingCategoryId",
                        column: x => x.ToppingCategoryId,
                        principalTable: "ToppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "All the toppings offered.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary Key unique identifier"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Users - the user who made the order."),
                    OrderStatus = table.Column<int>(type: "int", nullable: false, comment: "Current status of the order."),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, comment: "Price of the order."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time at which the order was created."),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Addresses - location where the order was supposed to be delivered.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "All the users' orders in the database.");

            migrationBuilder.CreateTable(
                name: "ShoppingCartsPizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary Key unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasePizzaId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to base Pizza"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to User. Indicates whose shopping cart this pizza is in."),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Quantity of this item in cart"),
                    PizzaComponentsJson = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "JSON serialized pizza data")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartsPizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartsPizzas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartsPizzas_Pizzas_BasePizzaId",
                        column: x => x.BasePizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Items in user's shopping cart");

            migrationBuilder.CreateTable(
                name: "UsersPizzas",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Users"),
                    PizzaId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Pizzas")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPizzas", x => new { x.UserId, x.PizzaId });
                    table.ForeignKey(
                        name: "FK_UsersPizzas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersPizzas_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A many-to-many mapping entity between User and Pizza, showing pizza entities which have been marked as favorite by users");

            migrationBuilder.CreateTable(
                name: "PizzasToppings",
                columns: table => new
                {
                    PizzaId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Pizzas, part of composite Primary Key."),
                    ToppingId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Toppings, part of composite Primary Key.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzasToppings", x => new { x.ToppingId, x.PizzaId });
                    table.ForeignKey(
                        name: "FK_PizzasToppings_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzasToppings_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A many-to-many mapping entity between Pizza and Toppings, used to show which toppings are contained in which pizzas.");

            migrationBuilder.CreateTable(
                name: "OrdersDesserts",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Orders, part of composite Primary Key."),
                    DessertId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Desserts, part of composite Primary Key."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Dessert quantity ordered."),
                    PricePerItemAtPurchase = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Price of the pizza at the time of purchase, used for total price calculations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDesserts", x => new { x.OrderId, x.DessertId });
                    table.ForeignKey(
                        name: "FK_OrdersDesserts_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersDesserts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A many-to-many mapping entity used to show which desserts appear in which orders.");

            migrationBuilder.CreateTable(
                name: "OrdersDrinks",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Orders, part of composite Primary Key."),
                    DrinkId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Drinks, part of composite Primary Key."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Ordered drink quantity"),
                    PricePerItemAtPurchase = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Price of the drink at the time of purchase, used for total price calculations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDrinks", x => new { x.OrderId, x.DrinkId });
                    table.ForeignKey(
                        name: "FK_OrdersDrinks_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersDrinks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A many-to-many mapping entity used to show which drinks appear in which orders.");

            migrationBuilder.CreateTable(
                name: "OrdersPizzas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Primary Key for OrderPizza. "),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to Orders. Shows which Order this pizza was used in."),
                    BasePizzaId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Pizzas. This points to the original pizza the OrderPizza was based on."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of pizzas in the order."),
                    PricePerItemAtPurchase = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Price of the pizza at the time of purchase, used for total price calculations."),
                    DoughId = table.Column<int>(type: "int", nullable: false, comment: "Dough used for this specific order pizza"),
                    SauceId = table.Column<int>(type: "int", nullable: true, comment: "Sauce used for this specific order Pizza. Can be null.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersPizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersPizzas_Doughs_DoughId",
                        column: x => x.DoughId,
                        principalTable: "Doughs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdersPizzas_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersPizzas_Pizzas_BasePizzaId",
                        column: x => x.BasePizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersPizzas_Sauces_SauceId",
                        column: x => x.SauceId,
                        principalTable: "Sauces",
                        principalColumn: "Id");
                },
                comment: "A many-to-many mapping entity used to show which pizzas appear in which orders. ");

            migrationBuilder.CreateTable(
                name: "OrderPizzaTopping",
                columns: table => new
                {
                    OrderPizzaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign Key to OrderPizzas, part of composite Primary Key."),
                    ToppingId = table.Column<int>(type: "int", nullable: false, comment: "Foreign Key to Toppings, part of composite Primary Key."),
                    PriceAtPurchase = table.Column<decimal>(type: "decimal(8,2)", nullable: false, comment: "Price of the topping at the time of purchase")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPizzaTopping", x => new { x.OrderPizzaId, x.ToppingId });
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_OrdersPizzas_OrderPizzaId",
                        column: x => x.OrderPizzaId,
                        principalTable: "OrdersPizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Toppings for a specific pizza in an order");

            migrationBuilder.InsertData(
                table: "Desserts",
                columns: new[] { "Id", "Description", "ImageUrl", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Rich cheesecake with blueberry jam! We could eat this all day!", null, false, "Cheesecake", 5m },
                    { 2, "Home baked with our special dough recipe and luxurious dark chocolate! Yum!", null, false, "Chocolate Brownie", 5m },
                    { 3, "Home is where the heart is. Or where the best apple pie is. We're still not sure...", null, false, "Apple Pie", 6m },
                    { 4, "Vanilla Ice Cream. Strawberry syrup. Need we say more?", null, false, "Vanilla Strawberry Ice Cream", 5m }
                });

            migrationBuilder.InsertData(
                table: "Doughs",
                columns: new[] { "Id", "Description", "IsDeleted", "Price", "Type" },
                values: new object[,]
                {
                    { 1, "Our classic white dough recipe everyone knows and loves!", false, 7.5m, "White" },
                    { 2, "Our special gluten-free dough!", false, 7.5m, "Gluten-free" },
                    { 3, "Our wholegrain dough packs additional fiber and protein for you fitness freaks out there!", false, 7.5m, "Wholegrain" }
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "Description", "ImageUrl", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "The refreshing original taste of Coca Cola!", null, false, "Coca Cola 500ml", 3m },
                    { 2, "The classic orange Fanta!", null, false, "Fanta 500ml", 3m },
                    { 3, "Who can say no to a pizza and sprite combo?", null, false, "Sprite 500ml", 3m },
                    { 4, "Salty and delicious - the perfect drink for a hearty lunch!", null, false, "Ayran 1L", 2.5m },
                    { 5, "100% flavor, 0 calories! Sounds like a bargain to us!", null, false, "Coca Cola Zero 500ml", 3m }
                });

            migrationBuilder.InsertData(
                table: "Sauces",
                columns: new[] { "Id", "Description", "IsDeleted", "Price", "Type" },
                values: new object[,]
                {
                    { 1, "Our signature tomato sauce with a special blend of herbs and spices that everyone knows and loves!", false, 1m, "Tomato" },
                    { 2, "Heavy cream sauce for rich and creamy pizzas. Did we mention it's very creamy?", false, 1m, "Cream" },
                    { 3, "Our custom made BBQ sauce with rich sweet and smokey aromas. Perfect for meaty pizzas!", false, 1m, "BBQ" },
                    { 4, "Olive oil, basil and garlic. We DARE you to think of a better flavor combination!", false, 1m, "Pesto" }
                });

            migrationBuilder.InsertData(
                table: "ToppingCategories",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "Meats" },
                    { 2, false, "Cheeses" },
                    { 3, false, "Vegetables" }
                });

            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "Id", "Description", "IsDeleted", "Name", "Price", "ToppingCategoryId" },
                values: new object[,]
                {
                    { 1, "A spicy, cured Italian-American sausage with a bold, savory flavor and a slightly crispy texture when baked.", false, "Pepperoni", 1m, 1 },
                    { 2, "Smoky, crispy and irresistibly delicious, bacon makes everything better - especially pizza!", false, "Bacon", 1m, 1 },
                    { 3, "Spicy, smoky Spanish sausage that kicks pizza up a notch - a flavor fiesta in every bite.", false, "Chorizo", 1m, 1 },
                    { 4, "Creamy, melty, stretchy perfection. Pizza without mozzarella is just sad bread.", false, "Mozzarella", 1m, 2 },
                    { 5, "Sharp, tangy, and gloriously gooey. Cheddar brings a bold twist to pizza that basic cheeses can't match.", false, "Cheddar", 1m, 2 },
                    { 6, "Salty, nutty, and irresistibly savory. Parmesan is the finishing touch that elevates pizza from good to gourmet.", false, "Parmesan", 1m, 2 },
                    { 7, "Velvety, indulgent, and irresistibly smooth. Philadelphia cheese turns pizza into a decadent delight.", false, "Philadelphia", 1m, 2 },
                    { 8, "Nature’s way of saying, ‘Yeah, this pizza needed more color.’", false, "Bell Peppers", 1m, 3 },
                    { 9, "The pizza topping that makes vegetarians and carnivores high-five!", false, "Mushrooms", 1m, 3 },
                    { 10, "Pizza’s way of keeping first dates interesting.", false, "Onions", 1m, 3 },
                    { 11, "Tiny, salty, and judging you for picking them off.", false, "Olives", 1m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaTopping_ToppingId",
                table: "OrderPizzaTopping",
                column: "ToppingId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDesserts_DessertId",
                table: "OrdersDesserts",
                column: "DessertId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDrinks_DrinkId",
                table: "OrdersDrinks",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_BasePizzaId",
                table: "OrdersPizzas",
                column: "BasePizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_DoughId",
                table: "OrdersPizzas",
                column: "DoughId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_OrderId",
                table: "OrdersPizzas",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPizzas_SauceId",
                table: "OrdersPizzas",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_CreatorUserId",
                table: "Pizzas",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_DoughId",
                table: "Pizzas",
                column: "DoughId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_SauceId",
                table: "Pizzas",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzasToppings_PizzaId",
                table: "PizzasToppings",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsDesserts_DessertId",
                table: "ShoppingCartsDesserts",
                column: "DessertId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsDrinks_DrinkId",
                table: "ShoppingCartsDrinks",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_BasePizzaId",
                table: "ShoppingCartsPizzas",
                column: "BasePizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartsPizzas_UserId",
                table: "ShoppingCartsPizzas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Toppings_ToppingCategoryId",
                table: "Toppings",
                column: "ToppingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPizzas_PizzaId",
                table: "UsersPizzas",
                column: "PizzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrderPizzaTopping");

            migrationBuilder.DropTable(
                name: "OrdersDesserts");

            migrationBuilder.DropTable(
                name: "OrdersDrinks");

            migrationBuilder.DropTable(
                name: "PizzasToppings");

            migrationBuilder.DropTable(
                name: "ShoppingCartsDesserts");

            migrationBuilder.DropTable(
                name: "ShoppingCartsDrinks");

            migrationBuilder.DropTable(
                name: "ShoppingCartsPizzas");

            migrationBuilder.DropTable(
                name: "UsersPizzas");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrdersPizzas");

            migrationBuilder.DropTable(
                name: "Toppings");

            migrationBuilder.DropTable(
                name: "Desserts");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "ToppingCategories");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Doughs");

            migrationBuilder.DropTable(
                name: "Sauces");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
