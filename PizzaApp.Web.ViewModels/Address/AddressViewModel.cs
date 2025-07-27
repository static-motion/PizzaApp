namespace PizzaApp.Web.ViewModels.Address
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public required string City { get; set; }

        public required string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }
    }
}
