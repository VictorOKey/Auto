using System;
using System.Collections.Generic;
using System.Linq;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Auto.Website.GraphQL.Queries;

public class UserQuery : ObjectGraphType
{
    private readonly IAutoDatabase db;

    public UserQuery(IAutoDatabase db)
    {
        this.db = db;

        Field<ListGraphType<UserGraphType>>("Users", "Запрос возвращающий всех пользователей",
            resolve: GetAllUsers);
        
        Field<UserGraphType>("User", "Запрос к конкретному пользователю по email",
            new QueryArguments(MakeNonNullStringArgument("email", "email пользователя")),
            resolve: GetUser);
        
        Field<ListGraphType<UserGraphType>>("UsersByName", "Запрос возвращающий всех пользователей по имени",
            new QueryArguments(MakeNonNullStringArgument("name", "Имя пользователя")),
            resolve: GetUsersByName);
    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }
    
    private IEnumerable<User> GetAllUsers(IResolveFieldContext<object> arg) => db.ListUsers();
    private User GetUser(IResolveFieldContext<object> arg)
    {
        var email = arg.GetArgument<string>("email");
        return db.FindUser(email);
    }

    private IEnumerable<User> GetUsersByName(IResolveFieldContext<object> arg)
    {
        var name = arg.GetArgument<string>("name");
        var user = db.ListUsers().Where(o => o.FirstName.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        return user;
    }
}