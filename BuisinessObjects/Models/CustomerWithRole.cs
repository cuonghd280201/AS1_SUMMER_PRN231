using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisinessObjects.Models
{
    public class CustomerWithRole: Customer
    {
        public CustomerWithRole(Customer customer) { 
                Email= customer.Email;
            CustomerId= customer.CustomerId;
        }
        public CustomerWithRole() { }
        public string CustomerRoleSring { get; set; }

    }
}
