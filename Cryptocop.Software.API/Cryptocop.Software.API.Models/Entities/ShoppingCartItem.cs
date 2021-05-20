namespace Cryptocop.Software.API.Models.Entities
{
    public class ShoppingCartItem
    {
        /*

        • ShoppingCartItem
            • Id (int)
            • ShoppingCartId (int)
            • ProductIdentifier (string)
            • Quantity (int)
            • UnitPrice (int)
        
        */
        public int Id { get; set; } // PK
        public int ShoppingCartId { get; set; } // FK
        public string ProductIdentifier { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        // Navigation Properties
        public ShoppingCart ShoppingCart { get; set; }
    }
}