using System;
using System.Collections.Generic;

namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Category
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string CategoryStatus { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
