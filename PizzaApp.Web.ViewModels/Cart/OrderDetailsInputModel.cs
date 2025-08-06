namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class OrderDetailsInputModel
    {
        [Required]
        public int? AddressId { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone number is required.")]
        public required string PhoneNumber { get; set; }

        public string? Comment { get; set; }
    }
}