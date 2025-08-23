namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class PizzaSeedingService : IPizzaSeedingService
    {
        private UserManager<User> _userManager;
        private IPizzaRepository _pizzaRepository;
        public PizzaSeedingService(UserManager<User> userManager, IPizzaRepository pizzaRepository)
        {
            this._userManager = userManager;
            this._pizzaRepository = pizzaRepository;
        }

        public async Task SeedPizzas()
        {
            User? admin = await this._userManager.FindByEmailAsync("admin@pizzahub.com")
                ?? throw new InvalidOperationException("Could not find admin user for pizza seeding.");

            List<Pizza> generatedPizzas = GeneratePizzas(admin.Id);
            IEnumerable<Pizza> existing = await this._pizzaRepository
                .IgnoreFiltering()
                .DisableTracking()
                .GetAllBasePizzasAsync();

            if (existing.Count() == 0)
            {
                await this._pizzaRepository.AddRangeAsync(generatedPizzas);
                await this._pizzaRepository.SaveChangesAsync();
            }
        }
        // IMAGES ARE COMMENTED OUT DUE TO HIGH RESOLUTION HINDERING RENDERING PERFORMANCE. UGLY BUT FASTER.
        private static List<Pizza> GeneratePizzas(Guid userId)
        {
            var pizzas = new List<Pizza>
            {
                new Pizza
                {
                    Name = "Classic Margherita",
                    Description = "A timeless classic with fresh tomato sauce and mozzarella.",
                    DoughId = 1,
                    SauceId = 1,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=1169&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Pepperoni Feast",
                    Description = "Loaded with spicy pepperoni and extra cheese.",
                    DoughId = 1,
                    SauceId = 1,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 1 },
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1534308983496-4fabb1a015ee?fm=jpg&q=60&w=3000&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Nnx8cGVwcGVyb25pJTIwcGl6emF8ZW58MHx8MHx8fDA%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "BBQ Bacon Bliss",
                    Description = "Smoky BBQ sauce with bacon and cheddar.",
                    DoughId = 2,
                    SauceId = 3,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 2 },
                        new PizzaTopping { ToppingId = 5 }
                    },
                    //ImageUrl = "https://images.pexels.com/photos/7813578/pexels-photo-7813578.jpeg",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Veggie Delight",
                    Description = "A garden-fresh pizza with veggies and pesto sauce.",
                    DoughId = 3,
                    SauceId = 4,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 8 },
                        new PizzaTopping { ToppingId = 9 },
                        new PizzaTopping { ToppingId = 11 }
                    },
                    //ImageUrl = "https://plus.unsplash.com/premium_photo-1722945691819-e58990e7fb27?q=80&w=1442&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Creamy Chorizo",
                    Description = "Rich cream sauce with spicy chorizo and mozzarella.",
                    DoughId = 1,
                    SauceId = 2,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 3 },
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.pexels.com/photos/9957551/pexels-photo-9957551.jpeg",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Four Cheese Heaven",
                    Description = "A cheesy masterpiece with four types of cheese.",
                    DoughId = 2,
                    SauceId = 1,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 4 },
                        new PizzaTopping { ToppingId = 5 },
                        new PizzaTopping { ToppingId = 6 },
                        new PizzaTopping { ToppingId = 7 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1617470702892-e01504297e84?q=80&w=1744&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Mushroom Mania",
                    Description = "Mushroom lovers' dream with creamy sauce.",
                    DoughId = 3,
                    SauceId = 2,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 9 },
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1692737580563-7ba2d896f0f6?q=80&w=1230&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Spicy Pepperoni",
                    Description = "Extra spicy pepperoni with BBQ sauce.",
                    DoughId = 1,
                    SauceId = 3,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 1 },
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1534308983496-4fabb1a015ee?q=80&w=1176&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
                new Pizza
                {
                    Name = "Olive Overload",
                    Description = "For olive enthusiasts with a tomato base.",
                    DoughId = 2,
                    SauceId = 1,
                    CreatorUserId = userId,
                    PizzaType = PizzaType.AdminPizza,
                    Toppings = new List<PizzaTopping>
                    {
                        new PizzaTopping { ToppingId = 11 },
                        new PizzaTopping { ToppingId = 4 }
                    },
                    //ImageUrl = "https://images.unsplash.com/photo-1458642849426-cfb724f15ef7?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    IsDeleted = false
                },
            };

            return pizzas;
        }
    }
}

