using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class AddressInputModel
    {
        /*
        
        • AddressInputModel
            • StreetName* (string)
            • HouseNumber* (string)
            • ZipCode* (string)
            • Country* (string)
            • City* (string)

        */
        
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
    }
}