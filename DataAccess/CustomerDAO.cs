using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerDAO
    {
        private static CustomerDAO instance = null;
        public static readonly object instanceLock = new object();
        private FUFlowerBouquetManagementContext db = new FUFlowerBouquetManagementContext();
        private CustomerDAO() { }

        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }
        public Customer GetDefaultCustomer()
        {
            return JsonConvert.DeserializeObject<Customer>(FlowerApiConfiguration1.DefaultAccount);
        }



        private static class FlowerApiConfiguration1
        {
            #region Private Members to get Configuration
            private static IConfigurationRoot GetConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true);
                return builder.Build();
            }
            #endregion

            public static string ConnectionString => GetConfiguration().GetConnectionString("Flower");

            public static string DefaultAccount => System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = GetConfiguration()["Account:DefaultAccount:Email"],
                Password = GetConfiguration()["Account:DefaultAccount:Password"]
            });

        }
        public async Task<Customer> Login(string email, string password)
        {
            var db = new FUFlowerBouquetManagementContext();
            IEnumerable<Customer> customers = await db.Customers.ToListAsync();
            customers = customers.Prepend(GetDefaultCustomer());
            return customers.SingleOrDefault(customer => customer.Email.ToLower().Equals(email.ToLower())
                                    && customer.Password.Equals(password));
        }

        public Customer CheckLogin(string email, string password)
        {
            var db = new FUFlowerBouquetManagementContext();
            try
            {
                Customer customer = db.Customers.SingleOrDefault(customer => customer.Email.ToLower().Equals(email.ToLower())
                                    && customer.Password.Equals(password));
                if(customer != null)
                {
                    return customer;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<Customer>> GetCustomer()
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Customers.ToListAsync();
        }

        private async Task<int> GetCustomerIncrease()
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Customers.MaxAsync(cus => cus.CustomerId) + 1;
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Customers.SingleOrDefaultAsync(cus => cus.CustomerId == customerId);
        }

        public async Task<Customer> GetCustomer(string Email)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Customers.SingleOrDefaultAsync(member => member.Email.ToLower().Equals(Email.ToLower()));
        }

        public async Task<Customer> AddCustomer(Customer newCustomer)
        {
            if (await GetCustomer(newCustomer.Email) != null)
            {
                throw new ApplicationException($"Member with email {newCustomer.Email} is existed!!");
            }
            var db = new FUFlowerBouquetManagementContext();
            newCustomer .CustomerId= await GetCustomerIncrease();
            await db.Customers.AddAsync(newCustomer);
            await db.SaveChangesAsync();
            return newCustomer;
        }

        public async Task<Customer> UpdateCustomer(Customer updatedCustomer)
        {
            Customer customer = await GetCustomer(updatedCustomer.CustomerId);
            if (customer == null)
            {
                throw new ApplicationException($"Customer with the ID does not exist!");
            }
            if (!updatedCustomer.Email.Equals(customer.Email))
            {
                throw new ApplicationException($"Email is not applicable to be updated!! Please try again...");
            }
            var db = new FUFlowerBouquetManagementContext();
            db.Customers.Update(updatedCustomer);
            await db.SaveChangesAsync();
            return updatedCustomer;
        }

        public async Task DeleteCustomer(int memberId)
        {
            Customer deletedCustomer = await GetCustomer(memberId);
            if (deletedCustomer == null)
            {
                throw new Exception($"Member with the ID {memberId} does not exist! Please check again...");
            }
            var db = new FUFlowerBouquetManagementContext();
            db.Customers.Remove(deletedCustomer);
            await db.SaveChangesAsync();
        }

    }
}
