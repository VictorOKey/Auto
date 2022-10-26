using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutation;

public class VehicleMutation: ObjectGraphType
{
    private readonly IAutoDatabase _db;

    public VehicleMutation(IAutoDatabase db)
    {
        this._db = db;
        
        Field<VehicleGraphType>(
            "createVehicle",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "registration"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "color"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "year"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "modelCode"}
            ),
            resolve: context =>
            {
                var registration = context.GetArgument<string>("registration");
                var color = context.GetArgument<string>("color");
                var year = context.GetArgument<int>("year");
                var modelCode = context.GetArgument<string>("modelCode");

                var vehicleModel = db.FindModel(modelCode);
                var vehicle = new Vehicle
                {
                    Registration = registration,
                    Color = color,
                    Year = year,
                    VehicleModel = vehicleModel,
                    ModelCode = vehicleModel.Code
                };
                _db.CreateVehicle(vehicle);
                return vehicle;
            }
        );
    }
}