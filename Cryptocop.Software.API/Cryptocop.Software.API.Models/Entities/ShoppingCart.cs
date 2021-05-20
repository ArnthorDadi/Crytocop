using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class ShoppingCart
    {
        /*

        • ShoppingCart
            • Id (int)
            • UserId (int)
        
        */
        public int Id { get; set; } // PK
        public int UserId { get; set; } // FK
        // Navigation Properties
        public User User { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}