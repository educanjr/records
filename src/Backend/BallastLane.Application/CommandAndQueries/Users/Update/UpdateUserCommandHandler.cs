
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;

namespace BallastLane.Application.CommandAndQueries.Users.Update;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<DomainResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.id, cancellationToken);

        if (user is null)
        {
            return DomainResult.Failure(UserErrors.NotFound(request.id));
        }

        var updateResult = user.ChangeName(
            request.FirstName,
            request.LastName);
        if (updateResult.IsFailure)
        {
            return DomainResult.Failure(updateResult.Errors);
        }

        await _userRepository.Update(user);

        return DomainResult.Success();
    }
}
