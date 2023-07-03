
using BallastLane.Application.Abstractions;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;

namespace BallastLane.Application.CommandAndQueries.Records.Create;

internal sealed class CreateRecordCommandHandler : ICommandHandler<CreateRecordCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRecordRepository _recordRepository;

    public CreateRecordCommandHandler(IUserRepository userRepository, IRecordRepository recordRepository)
    {
        _userRepository = userRepository;
        _recordRepository = recordRepository;
    }

    public async Task<DomainResult<Guid>> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
    {
        var creator = await _userRepository.GetUserByIdAsync(request.CreatorId);
        if(creator is null)
        {
            return DomainResult.Failure<Guid>(RecordErrors.CreatorNotFound);
        }

        var recordResult = Record.Create(Guid.NewGuid(), creator, request.Title, request.Description);

        if (recordResult.IsFailure)
        {
            return DomainResult.Failure<Guid>(recordResult.Errors);
        }

        await _recordRepository.Add(recordResult.Value);
        return recordResult.Value.Id;
    }
}
