using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class User
    {
        /*

        • User
            • Id (int)
            • FullName (string)
            • Email (string)
            • HashedPassword (string)
        
        */
        public int Id { get; set; } // PK
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        // Navigation Properties
        public ShoppingCart ShoppingCart { get; set; }
        public List<Address> Addresses { get; set; }
        public List<PaymentCard> PaymentCards { get; set; }
        public List<Order> Orders { get; set; }
    }
}