using System;
using System.Collections.Generic;
using System.Linq;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Queries;

public class VehicleQuery : ObjectGraphType {
    private readonly IAutoDatabase db;

    public VehicleQuery(IAutoDatabase db) {
        this.db = db;

        Field<ListGraphType<VehicleGraphType>>("Vehicles", "Запрос возвращающий все автомобили",
            resolve: GetAllVehicles);

        Field<VehicleGraphType>("Vehicle", "Запрос к конкретному автомобилю",
            new QueryArguments(MakeNonNullStringArgument("registration", "Номера машины")),
            resolve: GetVehicle);

        Field<ListGraphType<VehicleGraphType>>("VehiclesByColor", "Запрос возвращающий все машины с выбранным цветом",
            new QueryArguments(MakeNonNullStringArgument("color", "Имя цвета")),
            resolve: GetVehiclesByColor);
    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => db.ListVehicles();

    private Vehicle GetVehicle(IResolveFieldContext<object> context) {
        var registration = context.GetArgument<string>("registration");
        return db.FindVehicle(registration);
    }

    private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
        var color = context.GetArgument<string>("color");
        var vehicles = db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        return vehicles;
    }
}