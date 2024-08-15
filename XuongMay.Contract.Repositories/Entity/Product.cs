using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.Contract.Repositories.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
