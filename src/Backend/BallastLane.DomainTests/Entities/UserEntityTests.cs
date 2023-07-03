
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Errors;
using BallastLane.DomainTests.Utils;
using FluentAssertions;

namespace BallastLane.DomainTests.Entities;

public class UserEntityTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";

    #region Create Method Tests
    private const string ValidEmail = "testc@email.com";
    private const string ValidPassword = "password";
    private readonly Guid ValidId = Guid.NewGuid();

    [Fact]
    public void Create_Method_Should_Return_Success()
    {
        DomainResult<User> result = User.Create(ValidId, ValidEmail, ValidFirstName, ValidLastName, ValidPassword);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(InvalidUserPropertyType.Empty)]
    [InlineData(InvalidUserPropertyType.TooLong)]
    [InlineData(InvalidUserPropertyType.EmailInvalidFormat)]
    public void Create_Method_Should_Return_Failure_On_Invalid_Email(InvalidUserPropertyType invalidType)
    {
        var invalidEmail = string.Empty;

        switch (invalidType)
        {
            case InvalidUserPropertyType.TooLong: 
                invalidEmail = ValueGeneratorUtility.GenerateString(UserErrors.EmailMaxLength / 2) 
                    + "@" 
                    + ValueGeneratorUtility.GenerateString(UserErrors.EmailMaxLength / 2)
                    + ".com";
                break;
            case InvalidUserPropertyType.EmailInvalidFormat:
                invalidEmail = "testcemail.com";
                break;
        }

        DomainResult<User> result = User.Create(ValidId, invalidEmail, ValidFirstName, ValidLastName, ValidPassword);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(InvalidUserPropertyType.Empty)]
    [InlineData(InvalidUserPropertyType.TooLong)]
    public void Create_Method_Should_Return_Failure_On_Invalid_FirstName(InvalidUserPropertyType invalidType)
    {
        var invalidFirstname = invalidType == InvalidUserPropertyType.TooLong
                ? ValueGeneratorUtility.GenerateString(UserErrors.FirstNameMaxLength + 1)
                : string.Empty;

        DomainResult<User> result = User.Create(ValidId, ValidEmail, invalidFirstname, ValidLastName, ValidPassword);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(InvalidUserPropertyType.Empty)]
    [InlineData(InvalidUserPropertyType.TooLong)]
    public void Create_Method_Should_Return_Failure_On_Invalid_LastName(InvalidUserPropertyType invalidType)
    {
        var invalidLastname = invalidType == InvalidUserPropertyType.TooLong
                ? ValueGeneratorUtility.GenerateString(UserErrors.LastNameMaxLength + 1)
                : string.Empty;

        DomainResult<User> result = User.Create(ValidId, ValidEmail, ValidFirstName, invalidLastname, ValidPassword);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_Method_Should_Return_Failure_On_Empty_Password()
    {
        var invalidPassword = string.Empty;

        DomainResult<User> result = User.Create(ValidId, ValidEmail, ValidFirstName, ValidLastName, invalidPassword);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }
    #endregion

    #region ChangeName Method Tests
    [Fact]
    public void ChangeName_Method_Should_Return_Seccess()
    {
        var user = new User(ValidId, ValidEmail, string.Empty, string.Empty, ValidPassword);

        DomainResult result = user.ChangeName(ValidFirstName, ValidLastName);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        user.FirstName.Should().Be(ValidFirstName);
        user.LastName.Should().Be(ValidLastName);
    }

    [Theory]
    [InlineData(InvalidUserPropertyType.Empty)]
    [InlineData(InvalidUserPropertyType.TooLong)]
    public void ChangeName_Method_Should_Return_Failure_On_Invalid_FirstName(InvalidUserPropertyType invalidType)
    {
        var user = new User(ValidId, ValidEmail, ValidFirstName, ValidLastName, ValidPassword);
        var invalidFirstname = invalidType == InvalidUserPropertyType.TooLong
                ? ValueGeneratorUtility.GenerateString(UserErrors.FirstNameMaxLength + 1)
                : string.Empty;

        DomainResult result = user.ChangeName(invalidFirstname, "test");

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        user.FirstName.Should().Be(ValidFirstName);
        user.LastName.Should().Be(ValidLastName);
    }

    [Theory]
    [InlineData(InvalidUserPropertyType.Empty)]
    [InlineData(InvalidUserPropertyType.TooLong)]
    public void ChangeName_Method_Should_Return_Failure_On_Invalid_LastName(InvalidUserPropertyType invalidType)
    {
        var user = new User(ValidId, ValidEmail, ValidFirstName, ValidLastName, ValidPassword);
        var invalidLastname = invalidType == InvalidUserPropertyType.TooLong
                ? ValueGeneratorUtility.GenerateString(UserErrors.LastNameMaxLength + 1)
                : string.Empty;

        DomainResult result = user.ChangeName("test", invalidLastname);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        user.FirstName.Should().Be(ValidFirstName);
        user.LastName.Should().Be(ValidLastName);
    }
    #endregion
}

public enum InvalidUserPropertyType
{
    TooLong,
    Empty,
    EmailInvalidFormat
}