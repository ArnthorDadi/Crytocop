using Cryptocop.Software.API.Models.HyperMedia;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class ShoppingCartItemDto : HyperMediaModel
    {
        /*

        • ShoppingCartItemDto
            • Id (int)
            • ProductIdentifier (string)
            • Quantity (float)
            • UnitPrice (float)
            • TotalPrice (float)

        */
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public string ProductIdentifier { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
    }
}