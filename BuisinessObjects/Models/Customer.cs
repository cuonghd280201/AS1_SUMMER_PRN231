using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuisinessObjects.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        [Required(ErrorMessage ="Email not empty")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Customer name not empty")]
        [Display(Name= "Customer Name")]
        public string CustomerName { get; set; } = null!;
        [Required(ErrorMessage = "City not empty")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Country not empty")]
        public string Country { get; set; } = null!;
        [Required(ErrorMessage = "Password not empty")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Birthday not empty")]
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
