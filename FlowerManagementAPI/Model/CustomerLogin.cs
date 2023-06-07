using System.ComponentModel.DataAnnotations;

namespace FlowerManagementAPI.Model
{
    public class CustomerLogin
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
