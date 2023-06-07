using BuisinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlowerManagementAPI
{
    public static class ApiUtils
    {
        public static CustomerRole GetCustomerRole(this ClaimsPrincipal user)
        {
            return Enum.Parse<CustomerRole>(user.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value);
        }

        public static int GetCustomerId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        }
    }
}
