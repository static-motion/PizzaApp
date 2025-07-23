namespace PizzaApp.Services.Common.Dtos
{
    public class PizzaCartDto
    {
        public int PizzaId { get; set; }

        public int DoughId { get; set; }

        public int? SauceId { get; set; }

        public ICollection<int> SelectedToppingsIds { get; set; }
            = new List<int>();
        public int Quantity { get; set; }
    }
}
