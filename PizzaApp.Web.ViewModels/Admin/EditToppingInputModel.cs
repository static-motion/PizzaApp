namespace PizzaApp.Web.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using static GCommon.EntityConstraints.Topping;
    public class EditToppingInputModel
    {
        public int Id { get; set; }

        public int ToppingCategoryId { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public required string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public required string Description { get; set; }

        [Range(1, 20)]
        [Required]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}