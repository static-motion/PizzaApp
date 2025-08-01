namespace PizzaApp.Web.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using static PizzaApp.GCommon.EntityConstraints.Sauce;
    public class EditSauceInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(TypeMaxLength, MinimumLength = TypeMinLength)]
        public required string Type { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public required string Description { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
