namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("Items in user's shopping cart")]
    public class ShoppingCartPizza : IEntity<int>
    {
        [Comment("Primary Key unique identifier")]
        public int Id { get; set; }

        [Comment("Foreign Key to base Pizza")]
        public int BasePizzaId { get; set; }

        public Pizza BasePizza { get; set; } = null!;

        [Comment("Foreign Key to User. Indicates whose shopping cart this pizza is in.")]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Comment("Quantity of this item in cart")]
        public int Quantity { get; set; }

        [Comment("JSON serialized pizza data")]
        public string? PizzaComponentsJson { get; set; }

        // Helper methods to work with components data
        public PizzaComponentsDto? GetComponentsFromJson()
        {
            if (string.IsNullOrEmpty(this.PizzaComponentsJson))
                return null;

            return JsonSerializer.Deserialize<PizzaComponentsDto>(this.PizzaComponentsJson);
        }

        public void SerializeComponentsToJson(PizzaComponentsDto? data)
        {
            if (data == null)
            {
                this.PizzaComponentsJson = null;
            }
            else
            {
                this.PizzaComponentsJson = JsonSerializer.Serialize(data);
            }
        }
    }
}