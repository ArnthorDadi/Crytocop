namespace Cryptocop.Software.API.Models.Entities
{
    public class OrderItem
    {
        /*

        • OrderItem
            • Id (int)
            • OrderId (int)
            • ProductIdentifier (string)
            • Quantity (int)
            • UnitPrice (int)
            • TotalPrice (int)

        */
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductIdentifier { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        // Navigation Properties
        public Order Order { get; set; }
    }
}