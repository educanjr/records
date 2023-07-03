
using BallastLane.Application.Abstractions;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;

namespace BallastLane.Application.CommandAndQueries.Records.Delete;

internal sealed class DeleteRecordCommandHandler : ICommandHandler<DeleteRecordCommand>
{
    private readonly IRecordRepository _recordRepository;
    private readonly IUserRepository _userRepository;

    public DeleteRecordCommandHandler(IRecordRepository recordRepository, IUserRepository userRepository)
    {
        _recordRepository = recordRepository;
        _userRepository = userRepository;
    }

    public async Task<DomainResult> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
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

        await _recordRepository.Delete(record.Id, creator.Id);
        return DomainResult.Success();
    }
}
