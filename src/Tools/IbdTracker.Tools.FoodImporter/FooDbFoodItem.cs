using System.Text.Json.Serialization;

namespace IbdTracker.Tools.FoodImporter
{
    public class FooDbFoodItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}