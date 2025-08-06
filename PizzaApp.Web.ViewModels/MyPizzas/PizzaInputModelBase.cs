namespace PizzaApp.Web.ViewModels.Pizzas
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static PizzaApp.GCommon.EntityConstraints.Pizza;

    public abstract class PizzaInputModelBase : ICreatePizzaInputModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public required string Name { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public int DoughId { get; set; }

        [Range(1, double.MaxValue)]
        public int? SauceId { get; set; }

        public virtual PizzaType PizzaType { get; }

        public decimal Price { get; set; }

        public HashSet<int> SelectedToppingIds { get; set; } = new();
    }
}
