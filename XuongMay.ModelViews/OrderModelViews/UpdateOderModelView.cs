

namespace XuongMay.ModelViews.OrderModelViews
{
    public class UpdateOrderModelView
    {
        public required string Status { get; set; }
        public required DateTime Deadline { get; set; }
        public DateTime? FinishDate { get; set; } 
        public string? AssignedAccountId { get; set; }
    }
}
