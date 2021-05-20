using System.Dynamic;
using System.Text.Json.Serialization;

namespace Cryptocop.Software.API.Models.HyperMedia
{
    public class HyperMediaModel
    {
        public HyperMediaModel() { Links = new ExpandoObject(); }
        [JsonPropertyName("_links")]
        public ExpandoObject Links { get; set; }
    }
}