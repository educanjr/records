
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Errors;
using BallastLane.DomainTests.Utils;
using FluentAssertions;

namespace BallastLane.DomainTests.Entities;

public class RecordEntityTests
{
    private const string ValidTitle = "Valid Title";
    private const string ValidDescription = "Valid Description";

    #region Create Method Tests
    private readonly Guid ValidId = Guid.NewGuid();
    private readonly User ValidCreator = ValueGeneratorUtility.GenerateUser();

    [Fact]
    public void Create_Method_Should_Return_Success()
    {
        DomainResult<Domain.Entities.Record> result = Domain.Entities.Record.Create(ValidId, ValidCreator, ValidTitle, ValidDescription);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Method_Should_Return_Failure_On_Invalid_Title()
    {
        var invalidTitle = string.Empty;

        DomainResult<Domain.Entities.Record> result = Domain.Entities.Record.Create(ValidId, ValidCreator, invalidTitle, ValidDescription);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_Method_Should_Return_Failure_On_Invalid_Description()
    {
        var invalidDescription = string.Empty;

        DomainResult<Domain.Entities.Record> result = Domain.Entities.Record.Create(ValidId, ValidCreator, ValidTitle, invalidDescription);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }
    #endregion

    #region ChangeTitle Method Tests
    [Fact]
    public void ChangeTitle_Method_Should_Return_Seccess()
    {
        var record = new Domain.Entities.Record(ValidId, ValidCreator, string.Empty, string.Empty);

        DomainResult result = record.ChangeTitle(ValidTitle);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        record.Title.Should().Be(ValidTitle);
        record.Description.Should().Be(string.Empty);
    }

    [Fact]
    public void ChangeTitle_Method_Should_Return_Failure_On_Invalid_Title()
    {
        var record = new Domain.Entities.Record(ValidId, ValidCreator, ValidTitle, ValidDescription);

        DomainResult result = record.ChangeTitle(string.Empty);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        record.Title.Should().Be(ValidTitle);
    }
    #endregion

    #region ChangeDescription Method Test
    [Fact]
    public void ChangeDescription_Method_Should_Return_Seccess()
    {
        var record = new Domain.Entities.Record(ValidId, ValidCreator, string.Empty, string.Empty);

        DomainResult result = record.ChangeDescription(ValidDescription);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        record.Title.Should().Be(string.Empty);
        record.Description.Should().Be(ValidDescription);
    }

    [Fact]
    public void ChangeDescription_Method_Should_Return_Failure_On_Invalid_Title()
    {
        var record = new Domain.Entities.Record(ValidId, ValidCreator, ValidTitle, ValidDescription);

        DomainResult result = record.ChangeDescription(string.Empty);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        record.Description.Should().Be(ValidDescription);
    }
    #endregion
}