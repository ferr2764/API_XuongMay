using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.Contract.Repositories.Entity
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public decimal Salary { get; set; }
        public ICollection<Order> AssignedOrders { get; set; } = new List<Order>();
    }
}
