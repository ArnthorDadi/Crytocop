using Cryptocop.Software.API.Models.HyperMedia;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class UserDto : HyperMediaModel
    {
        /*

        • UserDto
            • Id (int)
            • FullName (string)
            • Email (string)
            • TokenId (int)
        
        */
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TokenId { get; set; }
    }
}