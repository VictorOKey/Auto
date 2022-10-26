using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public class UserGraphType:ObjectGraphType<User>
{
    public UserGraphType() {
        Name = "user";
        Field(o => o.UserVehicle, nullable: false, type: typeof(VehicleGraphType))
            .Description("Автомобиль владельца");
        Field(o => o.FirstName);
        Field(o => o.LastName);
        Field(o => o.Email);
        Field(o => o.VehicleRegistration);
    }
}