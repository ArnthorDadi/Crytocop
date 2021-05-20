namespace Cryptocop.Software.API.Repositories.Helpers
{
    public class PaymentCardHelper
    {
        public static string MaskPaymentCard(string paymentCardNumber)
        {
            // TODO: Implement
            var last4Numbers = paymentCardNumber.Substring(paymentCardNumber.Length-4,4);
            var mask = "************";
            return mask + last4Numbers;
        }
    }
}