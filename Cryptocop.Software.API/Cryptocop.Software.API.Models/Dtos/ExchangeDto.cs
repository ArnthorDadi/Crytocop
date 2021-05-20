using System;
using Cryptocop.Software.API.Models.HyperMedia;
using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class ExchangeDto : HyperMediaModel
    {
        /*

        • ExchangeDto
            • Id (int)
            • Name (string)
            • Slug (string)
            • AssetSymbol (string)
            • PriceInUsd (nullable float)
            • LastTrade (nullable datetime)

        */
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
        [JsonProperty(PropertyName = "symbol")]
        public string AssetSymbol { get; set; }
        [JsonProperty(PropertyName = "price_usd")]
        public float? PriceInUsd { get; set; }
        [JsonProperty(PropertyName = "last_trade_at")]
        public DateTime? LastTrade { get; set; }
    }
}