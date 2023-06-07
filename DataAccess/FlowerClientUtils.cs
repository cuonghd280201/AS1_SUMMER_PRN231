using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class FlowerClientUtils
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<HttpResponseMessage> ApiRequest(FlowerHttpMethod method, string apiUrl, object bodyExtra = null)
        {
            switch (method)
            {
                case FlowerHttpMethod.GET:
                    return await client.GetAsync(apiUrl);
                case FlowerHttpMethod.POST:
                    return await client.PostAsJsonAsync(apiUrl, bodyExtra);
                case FlowerHttpMethod.PUT:
                    return await client.PutAsJsonAsync(apiUrl, bodyExtra);
                case FlowerHttpMethod.DELETE:
                    return await client.DeleteAsync(apiUrl);
                default:
                    return null;
            }
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            var claims = user.Claims;
            var role = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
            if (role.Value.Equals("ADMIN"))
            {
                return true;
            }
            return false;
        }
    }
}
