namespace Cryptocop.Software.API.Models.Entities
{
    public class JwtToken
    {
        /*

        • JwtToken
            • Id (int)
            • Blacklisted (int)
        
        */
        public int Id { get; set; }
        public bool Blacklisted { get; set; }
    }
}