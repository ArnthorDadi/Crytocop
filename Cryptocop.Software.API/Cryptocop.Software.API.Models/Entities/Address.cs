using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class Address
    {
        /*

        • Address
            • Id (int)
            • UserId (int)
            • StreetName (string)
            • HouseNumber (string)
            • ZipCode (string)
            • Country (string)
            • City (string)
        
        */
        public int Id { get; set; } // PK
        public int UserId { get; set; } // FK
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        // Navigation Properties
        public User User { get; set; }
    }
}