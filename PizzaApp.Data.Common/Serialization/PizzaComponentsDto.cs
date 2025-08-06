namespace PizzaApp.Data.Common.Serialization
{
    using System.Collections.Generic;
    using System.Text.Json;

    // Customization model for serialization
    public class PizzaComponentsDto
    {
        public int DoughId { get; set; }
        public int? SauceId { get; set; }
        public ICollection<int> SelectedToppings { get; set; } = new List<int>();


        public bool Equals(PizzaComponentsDto? other)
        {
            if (other is null)
                return false;

            return this.DoughId == other.DoughId
                && this.SauceId == other.SauceId
                && this.SelectedToppings.Count == other.SelectedToppings.Count
                && (this.SelectedToppings.Intersect(other.SelectedToppings)).Count() == this.SelectedToppings.Count;
        }
    }
}
