using Newtonsoft.Json;

namespace Auto.Data.Entities {
    public partial class User {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public string VehicleRegistration { get; set; }
        [JsonIgnore]
        public virtual Vehicle UserVehicle { get; set; }
    }
}