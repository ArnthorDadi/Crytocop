using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class OrderInputModel
    {
        /*
        
        • OrderInputModel
            • AddressId (int)
            • PaymentCardId (int)

        */
        
        public int AddressId { get; set; }
        public int PaymentCardId { get; set; }
    }
}