using BallastLane.Application.Abstractions;
using BallastLane.Application.Errors;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallastLane.Application.CommandAndQueries.Users.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordProvider _passwordProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordProvider = passwordProvider;
    }

    public async Task<DomainResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return DomainResult.Failure<string>(UserErrors.InvalidCredentials);
        }

        var isSame = _passwordProvider.ComparePasswords(request.Password, user.PasswordHash);
        if(!isSame) 
        {
            return DomainResult.Failure<string>(UserErrors.InvalidCredentials);
        }

        string token = _jwtProvider.Generate(user);
        return token;
    }
}
