using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class ShoppingCartItemInputModel
    {
        /*

        • ShoppingCartItemInputModel
            • ProductIdentifier* (string)
            • Quantity* (nullable float)
            • The range for this number is an include 0.01 to the float type maximum value

        */
        
        [Required]
        public string ProductIdentifier { get; set; }
        [Required]
        [Range(0.01, float.MaxValue)]
        public float? Quantity { get; set; }
    }
}