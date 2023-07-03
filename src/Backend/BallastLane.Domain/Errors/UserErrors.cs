
using BallastLane.Domain.Common;

namespace BallastLane.Domain.Errors;

public static class UserErrors
{
    public static readonly int EmailMaxLength = 255;
    public static readonly int FirstNameMaxLength = 50;
    public static readonly int LastNameMaxLength = 80;
    public static readonly int PasswordMaxLength = 24;
    public static readonly int PasswordMinLength = 8;

    public static readonly Error EmailEmpty = new(
            "Email.Empty",
            "Email is empty."
        );

    public static readonly Error EmailTooLong = new(
        "Email.TooLong",
        "Provided email exeed the amount of characters allowed."
    );

    public static readonly Error EmailIncorrectFormat = new(
        "Email.IncorrectFormat",
        "The format of the provided email is incorrect."
    );

    public static readonly Error FirstNameEmpty = new(
            "FirstName.Empty",
            "First name is empty."
        );

    public static readonly Error FirstNameTooLong = new(
        "FirstName.TooLong",
        "First name the amount of characters allowed."
    );

    public static readonly Error LastNameEmpty = new(
            "LastName.Empty",
            "Last name is empty."
        );

    public static readonly Error LastNameTooLong = new(
        "LastName.TooLong",
        "Last name exeed the amount of characters allowed."
    );

    public static readonly Error PasswordEmpty = new(
            "Password.Empty",
            "Provided parword is empty."
        );

    public static readonly Error PasswordTooLong = new(
            "Password.TooLong",
            "Provided parword is longer than permited."
        );

    public static readonly Error PasswordTooShort = new(
            "Password.TooShort",
            "Provided parword is shorter than permited."
        );
}
