using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Order
    {
        public string OrderId { get; set; } = null!;

        public DateOnly CreatedDate { get; set; }

        public DateOnly FinishDate { get; set; }

        public string Status { get; set; } = null!;

        public DateOnly Deadline { get; set; }

        public string AccountId { get; set; } = null!;

        public string AssignedAccountId { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;

        public virtual Account AssignedAccount { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
