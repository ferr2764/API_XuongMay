namespace XuongMay.ModelViews.OrderModelViews
{
    public class CreateOrderModelView
    {
        public required DateTime Deadline { get; set; }
        public required Guid AccountId { get; set; }  // Changed to Guid
    }
}
