using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XuongMay.ModelViews.ProductModelViews
{
    public class UpdateProductModelView
    {
        public required string ProductName { get; set; }
        public required string ProductSize { get; set; }
        public required string Status { get; set; }
        public required string CategoryId { get; set; }
    }
}
