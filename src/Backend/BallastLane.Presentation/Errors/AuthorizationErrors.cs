
using BallastLane.Domain.Common;

namespace BallastLane.Presentation.Errors;

internal static class AuthorizationErrors
{
    public static readonly Error InvalidPermissions = new(
            "Authorization.InvalidPermissions",
            "The current user don't have the required permissions to complete the operation."
        );

    public static readonly Error UnregistredUser = new(
            "Authorization.UnregistredUser",
            "Imposible to get the current authenticated user data."
        );
}
