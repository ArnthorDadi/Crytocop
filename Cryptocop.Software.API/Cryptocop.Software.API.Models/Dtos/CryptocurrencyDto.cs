using System.Text.Json.Serialization;
using Cryptocop.Software.API.Models.HyperMedia;
using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class CryptocurrencyDto : HyperMediaModel
    {
        /*

        • CryptocurrencyDto
            • Id (int)
            • Symbol (string)
            • Name (string)
            • Slug (string)
            • PriceInUsd (float)
            • ProjectDetails (string)
        
        */
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Slug { get; set; }
        [JsonProperty(PropertyName = "price_usd")]
        public float PriceInUsd { get; set; }
        [JsonProperty(PropertyName = "project_details")]
        public string ProjectDetails { get; set; }
    }
}