# PizzaApp

Final SoftUni ASP.NET Advanced June 2025 Project based on [Kristiyan Ivanov ](https://github.com/KrIsKa7a)'s
[ASP.NET-Core-Arch-Template](https://github.com/KrIsKa7a/ASP.NET-Core-Arch-Template)

---

## Overview

**PizzaApp** (also known as PizzaHub) is a pizza restaurant ordering and management web application designed as the final project for the SoftUni ASP.NET Advanced course (June 2025). It is built primarily with C# and ASP.NET, with supporting HTML for web interface components. The application demonstrates advanced concepts in ASP.NET Core development.

---

## Technologies

- **ASP.NET Core**
- **Entity Framework Core**
- **Microsoft SQL Server**
- **Microsoft Identity**
- **Bootstrap**
- **nUnit**
- **Moq**
---

## Project Structure

The repository is organized into multiple solution folders and projects:

- `PizzaApp.Data.Common`, `PizzaApp.Data.Models`, `PizzaApp.Data`: Data access layers and models
- `PizzaApp.GCommon`: Shared/common utilities
- `PizzaApp.IntegrationTests`, `PizzaApp.Services.Core.Tests`, `PizzaApp.Web.Tests`: Automated tests for core services and web layer
- `PizzaApp.Services.AutoMapping`, `PizzaApp.Services.Common`, `PizzaApp.Services.Core`: Service layers for business logic and mapping
- `PizzaApp.Web`, `PizzaApp.Web.Infrastructure`, `PizzaApp.Web.ViewModels`: Web interface, infrastructure, and view models

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/static-motion/PizzaApp.git
   ```
2. **Open with Visual Studio 2022+**
   - Open `PizzaApp.sln`.
3. **Configure the database**
   - Set up your connection string in the configuration files.
   - Run migrations if necessary.
4. **Run the application**
   - Start the solution and run the `PizzaApp.Web` project to launch the web interface.
5. **Testing**
   - Run tests from their corresponding test projects.

---

## Features

- Fully customizable pizzas - Users can change every ingredient based on their preferences
- Admin panel - change the price, name, description and availability of any pizza or ingredient.
- Dynamic pricing - pizza prices are based on their components and calcluated in real time
- Modern ASP.NET Core project structure
- Layered architecture
- Repository pattern for data persistence
- Services unit tests
- MIT Licensed
---

## Roadmap

- SignalR realtime order tracking
- AJAX calls for better UX
- UI Improvements
- Restaurants and locations
- Restaurant manager role

## License

This project is licensed under the [MIT License](https://github.com/static-motion/PizzaApp/blob/main/LICENSE).

---

## Contributing

Contributions, feedback, and suggestions are welcome!  
Please open an issue or pull request for any improvements or bug fixes.

---

## Author

[Valentin Mihov](https://github.com/static-motion)
