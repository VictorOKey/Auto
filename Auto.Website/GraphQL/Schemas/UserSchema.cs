using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.Mutation;
using Auto.Website.GraphQL.Queries;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Schemas;

public class UserSchema : Schema
{
    public UserSchema(IAutoDatabase db)
    {
        Query = new UserQuery(db);
        Mutation = new UserMutation(db);
    }
}