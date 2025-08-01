namespace PizzaApp.Web.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    using static GCommon.EntityConstraints.ToppingCategory;
    public class EditToppingCategoryInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public required string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
