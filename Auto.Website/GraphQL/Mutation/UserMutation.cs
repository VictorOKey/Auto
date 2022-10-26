using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutation;

public class UserMutation : ObjectGraphType
{
    private readonly IAutoDatabase _db;

    public UserMutation(IAutoDatabase db)
    {
        this._db = db;


        Field<UserGraphType>(
            "createUser",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "firstname" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "lastname" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "vehicleRegistration" }
            ),
            resolve: context =>
            {
                var name = context.GetArgument<string>("name");
                var surname = context.GetArgument<string>("surname");
                var email = context.GetArgument<string>("email");
                var vehicleRegistration = context.GetArgument<string>("vehicleRegistration");

                var userVehicle = db.FindVehicle(vehicleRegistration);
                var user = new User
                {
                    FirstName = name,
                    LastName = surname,
                    Email = email,
                    UserVehicle = userVehicle,
                    VehicleRegistration = userVehicle.Registration
                };
                _db.CreateUser(user);
                return user;
            });
    }
}