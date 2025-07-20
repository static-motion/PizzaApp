namespace PizzaApp.Web.ViewModels.Pizzas
{
    using System.ComponentModel.DataAnnotations;

    using static PizzaApp.GCommon.EntityConstraints.Pizza;

    public class PizzaInputModel
    {
        // TODO: Write error messages for validation attributes
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public required string Name { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public int DoughId { get; set; }

        public int? SauceId { get; set; }
    }
}
