using Cryptocop.Software.API.Models.HyperMedia;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class PaymentCardDto : HyperMediaModel
    {
        /*

        • PaymentCardDto
            • Id (int)
            • CardholderName (string)
            • CardNumber (string)
            • Month (int)
            • Year (int)
        
        */
        public int Id { get; set; }
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}