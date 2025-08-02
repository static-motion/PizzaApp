namespace PizzaApp.Web.ViewModels.Menu
{
    using PizzaApp.GCommon.Enums;
    using System.ComponentModel.DataAnnotations;

    public class MenuItemDetailsViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public MenuCategory Category { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        [Range(1, 20)]
        public int Quantity { get; set; }
    }
}
