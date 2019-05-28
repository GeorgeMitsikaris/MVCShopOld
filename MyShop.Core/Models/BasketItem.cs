namespace MyShop.Core.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}