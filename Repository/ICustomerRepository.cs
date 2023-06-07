using BuisinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICustomerRepository
    {
        public Customer LoginA(string email, string password); 
        Task<Customer> Login(string email, string password);
        Customer GetDefaultCustomer();
        Task<IEnumerable<Customer>> GetCustomer();
        Task<Customer> GetCustomer(int customerId);
        Task<Customer> GetCustomer(string Email);
        Task<Customer> AddCustomer(Customer newCustomer);
        Task<Customer> UpdateCustomer(Customer updatedCustomer);
        Task DeleteCustomer(int customerId);
    }
}
