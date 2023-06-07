using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuisinessObjects.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime? ShippedDate { get; set; }
        public decimal? Total { get; set; }
        public string? OrderStatus { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
