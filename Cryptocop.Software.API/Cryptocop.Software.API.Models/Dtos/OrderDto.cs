using System.Collections.Generic;
using Cryptocop.Software.API.Models.HyperMedia;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class OrderDto : HyperMediaModel
    {
        /*

        • OrderDto
            • Id (int)
            • Email (string)
            • FullName (string)
            • StreetName (string)
            • HouseNumber (string)
            • ZipCode (string)
            • Country (string)
            • City (string)
            • CardholderName (string)
            • CreditCard (string)
            • OrderDate (string)
                • Represented as 01.01.2020
            • TotalPrice (float)
            • OrderItems (list of OrderItemDto)
        
        */
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CardholderName { get; set; }
        public string CreditCard { get; set; }
        public string OrderDate { get; set; } // • Represented as 01.01.2020
        public float TotalPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}