using Cryptocop.Software.API.Models.HyperMedia;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class AddressDto : HyperMediaModel
    {
        /*

        • AddressDto
            • Id (int)
            • StreetName (string)
            • HouseNumber (string)
            • ZipCode (string)
            • Country (string)
            • City (string)
        
        */
        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}