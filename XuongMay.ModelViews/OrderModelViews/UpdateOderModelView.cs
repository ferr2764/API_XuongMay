using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuongMay.ModelViews.OrderModelViews
{
    public class UpdateOrderModelView
    {
        public required string Status { get; set; }
        public required DateTime Deadline { get; set; }
        public DateTime? FinishDate { get; set; } // Nullable in case it hasn't been finished yet
        public string? AssignedAccountId { get; set; } // Nullable if want to allow updates without reassigning
    }
}
