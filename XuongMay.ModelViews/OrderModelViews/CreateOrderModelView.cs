using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.ModelViews.OrderModelViews
{
    public class CreateOrderModelView
    {
        public required DateTime Deadline { get; set; }
        public required string AccountId { get; set; }
    }
}
