
using BallastLane.Domain.Common;
using BallastLane.Domain.Errors;
using BallastLane.Domain.Primitives;

namespace BallastLane.Domain.Entities;

public class Record : BaseEntity, IAuditableEntity
{
    public User Creator { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public Record(Guid id, User creator, string title, string description)
        : base(id)
    {
        Creator = creator;
        Title = title;
        Description = description;
    }

    public static DomainResult<Record> Create(Guid id, User creator, string title, string description)
    {
        var titleResult = DomainResult.Ensure(
            title,
            (x => !string.IsNullOrWhiteSpace(x), RecordErrors.TitleEmpty));

        var descriptionResult = DomainResult.Ensure(
             description,
             (x => !string.IsNullOrWhiteSpace(x), RecordErrors.DescriptionEmpty));

        if(titleResult.IsFailure || descriptionResult.IsFailure)
        {
            // 
            var errors = new List<Error>();
            errors.AddRange(titleResult.ValidErrors);
            errors.AddRange(descriptionResult.ValidErrors);
            return DomainResult.Failure<Record>(errors.ToArray());
        }

        var record = new Record(id, creator, title, description);
        return record;
    }

    public DomainResult ChangeTitle(string? title)
    {
        var titleResult = DomainResult.Ensure(
            title,
            (x => !string.IsNullOrWhiteSpace(x), RecordErrors.TitleEmpty));

        if(titleResult.IsSuccess) 
        {
            Title = title!.Trim();
            return DomainResult.Success();
        }

        return DomainResult.Failure(titleResult.Errors);
    }

    public DomainResult ChangeDescription(string? description)
    {
        var descriptionResult = DomainResult.Ensure(
             description,
             (x => !string.IsNullOrWhiteSpace(x), RecordErrors.DescriptionEmpty));

        if (descriptionResult.IsSuccess)
        {
            Description = description!.Trim();
            return DomainResult.Success();
        }

        return DomainResult.Failure(descriptionResult.Errors);
    }
}
