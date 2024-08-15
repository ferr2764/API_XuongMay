using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.Contract.Repositories.Entity
{
    public class OrderDetail
    {
        public int DetailId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int NumberOfProds { get; set; }
        public int Status { get; set; }
    }
}
