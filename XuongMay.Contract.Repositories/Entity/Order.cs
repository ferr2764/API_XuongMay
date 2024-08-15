using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.Contract.Repositories.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
