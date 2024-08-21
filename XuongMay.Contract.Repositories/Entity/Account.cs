using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Account
    {
        public string AccountId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public int Salary { get; set; }

        public virtual ICollection<Order> OrderAccounts { get; set; } = new List<Order>();

        public virtual ICollection<Order> OrderAssignedAccounts { get; set; } = new List<Order>();
    }
}
