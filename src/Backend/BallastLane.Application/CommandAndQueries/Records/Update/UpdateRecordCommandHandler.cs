
using BallastLane.Application.Abstractions;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;

namespace BallastLane.Application.CommandAndQueries.Records.Update;

internal sealed class UpdateRecordCommandHandler : ICommandHandler<UpdateRecordCommand>
{
    private readonly IRecordRepository _recordRepository;
    private readonly IUserRepository _userRepository;

    public UpdateRecordCommandHandler(IRecordRepository recordRepository, IUserRepository userRepository)
    {
        _recordRepository = recordRepository;
        _userRepository = userRepository;
    }

    public async Task<DomainResult> Handle(UpdateRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.Get(request.Id);
        if(record is null)
        {
            return DomainResult.Failure(RecordErrors.NotFound(request.Id));
        }

        var creator = await _userRepository.GetUserByIdAsync(request.CreatorId, cancellationToken);

        if (creator is null || creator.Id != record.Creator.Id)
        {
            return DomainResult.Failure(RecordErrors.CreatorNotFound);
        }

        var changeTitleResult = record.ChangeTitle(request.NewTitle);
        var changeDescResult = record.ChangeDescription(request.NewDescription);

        if(changeTitleResult.IsFailure || changeDescResult.IsFailure)
        {
            var errors = new List<Error>();
            errors.AddRange(changeTitleResult.ValidErrors);
            errors.AddRange(changeDescResult.ValidErrors);
            return DomainResult.Failure(errors.ToArray());
        }

        await _recordRepository.Update(record);
        return DomainResult.Success();
    }
}
