namespace PizzaApp.Web.ViewModels.Menu
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;

    public class CustomizePizzaInputModel
    {

        [Range(1, double.MaxValue)]
        public int Id { get; set; }

        [ValidateNever]
        public required string Name { get; set; }

        [Range(1, 20)]
        public int Quantity { get; set; }

        public string? Description { get; set; }

        [Range(1, double.MaxValue)]
        public int DoughId { get; set; }

        [Range(1, double.MaxValue)]
        public int? SauceId { get; set; }

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<int> SelectedToppingIds { get; set; } = new HashSet<int>();
    }
}
