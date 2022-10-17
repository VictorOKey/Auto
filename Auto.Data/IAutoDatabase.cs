using System.Collections.Generic;
using Auto.Data.Entities;

namespace Auto.Data {
	public interface IAutoDatabase {
		
		public int CountVehicles();
		public int CountUsers();
		public IEnumerable<Vehicle> ListVehicles();
		public IEnumerable<Manufacturer> ListManufacturers();
		public IEnumerable<Model> ListModels();
		public IEnumerable<User> ListUsers();

		public Vehicle FindVehicle(string registration);
		public Model FindModel(string code);
		public Manufacturer FindManufacturer(string code);
		public User FindUser(string email);
		public void CreateVehicle(Vehicle vehicle);
		public void UpdateVehicle(Vehicle vehicle);
		public void DeleteVehicle(Vehicle vehicle);
		public void CreateUser(User user);
		public void UpdateUser(User user);
		public void DeleteUser(User user);
	}
}
