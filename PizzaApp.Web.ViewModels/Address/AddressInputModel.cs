namespace PizzaApp.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    public class AddressInputModel
    {
        [Required]
        public string City { get; set; } = null!;

        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; } = null!;

        [Display(Name = "Address Line 2")]
        public string? AddressLine2 { get; set; }
    }
}
