namespace PizzaApp.Web.ViewModels.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using System.ComponentModel.DataAnnotations;

    using static PizzaApp.GCommon.EntityConstraints.Pizza;

    public interface ICreatePizzaInputModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public int DoughId { get; set; }

        [Range(1, double.MaxValue)]
        public int? SauceId { get; set; }

        public PizzaType PizzaType { get; }

        public HashSet<int> SelectedToppingIds { get; set; }
    }
}
