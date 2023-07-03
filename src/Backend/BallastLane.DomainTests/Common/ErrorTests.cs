
using BallastLane.Domain.Common;
using FluentAssertions;

namespace BallastLane.DomainTests.Common;

public class ErrorTests
{

    [Fact]
    public void Error_None_Should_BeEmpty()
    {
        Error errorNone = Error.None;

        errorNone.Should().NotBeNull();
        errorNone.Code.Should().BeEmpty();
        errorNone.Message.Should().BeEmpty();
    }

    [Fact]
    public void Error_NulValue_Should_Not_BeEmpty()
    {
        Error errorNullValue = Error.NullValue;

        errorNullValue.Should().NotBeNull();
        errorNullValue.Code.Should().NotBeEmpty();
        errorNullValue.Message.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Error_Validate_Equal_Operator(bool equal)
    {
        var error1 = new Error("testCode", "testDescription");
        var error2 = equal
            ? new Error("testCode", "testDescription")
            : new Error("testCodeDiff", "testDescription");

        var result = error1 == error2;

        if (equal)
        {
            result.Should().BeTrue();
        }
        else
        {
            result.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Error_Validate_Equal_Method(bool equal)
    {
        var error1 = new Error("testCode", "testDescription");
        var error2 = equal
            ? new Error("testCode", "testDescription")
            : new Error("testCodeDiff", "testDescription");

        var result = error1.Equals(error2);

        if (equal)
        {
            result.Should().BeTrue();
        }
        else
        {
            result.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Error_Validate_NotEqual_Operator(bool equal)
    {
        var error1 = new Error("testCode", "testDescription");
        var error2 = equal
            ? new Error("testCode", "testDescription")
            : new Error("testCodeDiff", "testDescription");

        var result = error1 != error2;

        if (equal)
        {
            result.Should().BeFalse();
        }
        else
        {
            result.Should().BeTrue();
        }
    }
}
