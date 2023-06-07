using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuisinessObjects.Models
{
    public partial class FlowerBouquet
    {
        public FlowerBouquet()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int FlowerBouquetId { get; set; }
        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string FlowerBouquetName { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int UnitsInStock { get; set; }
        [Required]
        public byte? FlowerBouquetStatus { get; set; }

        [Required]
        public int? SupplierId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
