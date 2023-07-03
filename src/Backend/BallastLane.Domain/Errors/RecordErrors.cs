
using BallastLane.Domain.Common;

namespace BallastLane.Domain.Errors;

public static class RecordErrors
{
    public static readonly Error TitleEmpty = new(
            "Record.EmptyTitle",
            "Record field Title is empty."
        );

    public static readonly Error DescriptionEmpty = new(
            "Record.EmptyDescription",
            "Record field Description is empty."
        );
}
