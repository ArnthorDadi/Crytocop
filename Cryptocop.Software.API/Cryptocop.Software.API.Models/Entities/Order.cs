using System;
using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class Order
    {
        /*

        • Order
            • Id (int)
            • Email (string)
            • FullName (string)
            • StreetName (string)
            • HouseNumber (string)
            • ZipCode (string)
            • Country (string)
            • City (string)
            • CardHolderName (string)
            • MaskedCreditCard (string)
            • OrderDate (datetime)
            • TotalPrice (int)
        */
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CardHolderName { get; set; }
        public string MaskedCreditCard { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalPrice { get; set; }
        // Navigation Properties
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}