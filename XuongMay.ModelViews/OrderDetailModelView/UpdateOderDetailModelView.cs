namespace XuongMay.ModelViews.OrderDetailModelView
{
    public class UpdateOrderDetailModelView
    {
        public required Guid ProductId { get; set; }  // Changed to Guid
        public required string Status { get; set; }
        public required int NumberOfProds { get; set; }
    }
}
