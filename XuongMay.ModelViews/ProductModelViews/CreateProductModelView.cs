namespace XuongMay.ModelViews.ProductModelViews
{
    public class CreateProductModelView
    {
        public required string ProductName { get; set; }
        public required string ProductSize { get; set; }
        public required Guid CategoryId { get; set; }  // Changed to Guid
    }
}
