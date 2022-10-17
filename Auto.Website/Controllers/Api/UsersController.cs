using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auto.Website.Controllers.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IAutoDatabase db;

        public UsersController(IAutoDatabase db)
        {
            this.db = db;
        }
        
        private dynamic Paginate(string url, int index, int count, int total) {
            dynamic links = new ExpandoObject();
            links.self = new { href = url };
            links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
            links.first = new { href = $"{url}?index=0&count={count}" };
            if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
            if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
            return links;
        }
        
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get(int index = 0, int count = 5) {
            var items = db.ListUsers().Skip(index).Take(count);
            var total = db.CountUsers();
            var _links = Paginate("/api/users", index, count, total);
            var _actions = new {
                create = new {
                    method = "POST",
                    type = "application/json",
                    name = "Create a new user",
                    href = "/api/users"
                },
                delete = new {
                    method = "DELETE",
                    name = "Delete a user",
                    href = "/api/users/{id}"
                }
            };
            var result = new {
                _links, _actions, index, count, total, items
            };
            return Ok(result);
        }
        [HttpGet("{email}")]
        public IActionResult Get(string email) {
            var user = db.FindUser(email);
            if (user == default) return NotFound();
            var json = user.ToDynamic();
            json._links = new {
                self = new { href = $"/api/user/{email}" },
                userVehicle = new { href = $"/api/vehicles/{user.VehicleRegistration}" }
            };
            json._actions = new {
                update = new {
                    method = "PUT",
                    href = $"/api/users/{email}",
                    accept = "application/json"
                },
                delete = new {
                    method = "DELETE",
                    href = $"/api/users/{email}"
                }
            };
            return Ok(json);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto dto) {
            var userVehicle = db.FindVehicle(dto.VehicleRegistration);
            var user = new User {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                VehicleRegistration = dto.VehicleRegistration,
                UserVehicle = userVehicle
            };
            db.CreateUser(user);
			
            return Ok(dto);
        }

        [HttpPut("{email}")]
        public IActionResult Put(string email, [FromBody] dynamic dto) {
            var userVehicleHref = dto._links.userVehicle.href;
            var userVehicleReg = VehiclesController.ParseRegistration(userVehicleHref);
            var userVehicle = db.FindVehicle(userVehicleReg);
            var user = new User {
                FirstName = dto.name,
                LastName = dto.surname,
                VehicleRegistration = userVehicle.Registration
            };
            db.UpdateUser(user);
            return Get(email);
        }
        
        [HttpDelete("{email}")]
        public IActionResult Delete(string email) {
            var user = db.FindUser(email);
            if (user == default) return NotFound();
            db.DeleteUser(user);
            return NoContent();
        }
    }
}