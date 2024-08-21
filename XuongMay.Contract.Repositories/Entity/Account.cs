using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Account
    {
        public Guid AccountId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int Salary { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Order> OrderAccounts { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrderAssignedAccounts { get; set; } = new List<Order>();
    }
}
