namespace Cryptocop.Software.API.Models.Entities
{
    public class PaymentCard
    {
        /*

        • PaymentCard
            • Id (int)
            • UserId (int)
            • CardholderName (string)
            • CardNumber (string)
            • Month (int)
            • Year (int)
        
        */
        public int Id { get; set; } // PK
        public int UserId { get; set; } // FK
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        // Navigation Properties
        public User User { get; set; }
    }
}