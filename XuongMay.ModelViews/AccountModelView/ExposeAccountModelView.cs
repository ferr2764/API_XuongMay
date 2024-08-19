using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XuongMay.ModelViews.AccountModelView
{
    public class ExposeAccountModelView
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string? Role { get; set; }

        public int? Salary { get; set; }

        public string? Status { get; set; }
    }
}
