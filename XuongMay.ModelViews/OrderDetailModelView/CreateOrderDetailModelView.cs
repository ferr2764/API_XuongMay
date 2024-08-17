using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.ModelViews.OrderDetailModelView
{
    public class CreateOrderDetailModelView
    {
        public required string OrderId { get; set; }
        public required string NumberOfProds { get; set; }
    }
}
