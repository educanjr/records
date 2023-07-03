
using BallastLane.Domain.Common;

namespace BallastLane.Application.Errors;

public static class RecordErrors
{
    public static readonly Error CreatorNotFound = new(
            "Record.CreatorNotFound",
            "The creator of the record is not a registered user."
        );

    public static readonly Func<Guid, Error> NotFound = id => new(
            "Record.NotFound",
            $"The record with id {id} don't exists in the system."
        );
}
