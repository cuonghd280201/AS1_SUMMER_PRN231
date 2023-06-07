using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<Customer> AddCustomer(Customer newCustomer)
        {
            return await CustomerDAO.Instance.AddCustomer(newCustomer);
        }

        public async Task DeleteCustomer(int customerId)
        {
             await CustomerDAO.Instance.DeleteCustomer(customerId);
        }

        public async Task<IEnumerable<Customer>> GetCustomer()
        {
            return await CustomerDAO.Instance.GetCustomer();
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            return await CustomerDAO.Instance.GetCustomer(customerId);
        }

        public async Task<Customer> GetCustomer(string Email)
        {
            return await CustomerDAO.Instance.GetCustomer(Email);
        }

        public  Customer GetDefaultCustomer()
        {
            return CustomerDAO.Instance.GetDefaultCustomer();   
        }

        public async Task<Customer> Login(string email, string password)
        {
           return await CustomerDAO.Instance.Login(email, password);
        }

        public Customer LoginA(string email, string password)
        {
            return CustomerDAO.Instance.CheckLogin(email, password);
        }

        public async Task<Customer> UpdateCustomer(Customer updatedCustomer)
        {
            return await CustomerDAO.Instance.UpdateCustomer(updatedCustomer);
        }
    }
}
