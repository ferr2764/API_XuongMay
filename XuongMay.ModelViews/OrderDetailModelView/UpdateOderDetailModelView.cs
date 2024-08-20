using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XuongMay.ModelViews.OrderDetailModelView
{
    public class UpdateOrderDetailModelView
    {
        public required string ProductId { get; set; }
        public required string Status { get; set; }
        public required int NumberOfProds { get; set; }
    }
}
