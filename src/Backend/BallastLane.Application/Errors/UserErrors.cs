
using BallastLane.Domain.Common;

namespace BallastLane.Application.Errors;

public static class UserErrors
{
    public static readonly Error EmailInUse = new(
            "User.EmailInUse",
            "The provided email is alredy in the system."
        );

    public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid."
        );

    public static readonly Func<Guid, Error> NotFound = id => new(
            "User.NotFound",
            $"The user with id {id} don't exists in the system."
        );
}
