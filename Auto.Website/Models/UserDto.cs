using System.ComponentModel.DataAnnotations;

namespace Auto.Website.Models
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Range(18,100)]
        public string VehicleRegistration { get; set; }
    }
}