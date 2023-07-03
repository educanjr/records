
using BallastLane.Application.Abstractions;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;

namespace BallastLane.Application.CommandAndQueries.Users.Create;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    private readonly IPasswordProvider _passwordProvider;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;

        _passwordProvider = passwordProvider;
    }

    public async Task<DomainResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var encryptedPassword = _passwordProvider.HashPassword(request.Password);

        var userResult = User.Create(
           Guid.NewGuid(),
           request.Email,
           request.FirstName,
           request.LastName,
           encryptedPassword);

        if (userResult.IsFailure)
        {
            return DomainResult.Failure<Guid>(userResult.Errors);
        }

        var isEmailUnique = await _userRepository.IsEmailUniqueAsync(userResult.Value.Email, cancellationToken);
        if (!isEmailUnique)
        {
            return DomainResult.Failure<Guid>(UserErrors.EmailInUse);
        }

        await _userRepository.Add(userResult.Value);

        return userResult.Value.Id;
    }
}
