namespace XuongMay.ModelViews.OrderDetailModelView
{
    public class CreateOrderDetailModelView
    {
        public required Guid OrderId { get; set; }  // Changed to Guid
        public required Guid ProductId { get; set; }  // Changed to Guid
        public required int NumberOfProds { get; set; }
    }
}
