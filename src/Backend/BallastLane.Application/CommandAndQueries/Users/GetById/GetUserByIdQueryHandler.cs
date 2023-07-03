
using BallastLane.Application.Abstractions;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Common;
using BallastLane.Application.Errors;

namespace BallastLane.Application.CommandAndQueries.Users.GetById;

internal sealed class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<DomainResult<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return DomainResult.Failure<GetUserByIdResponse>(UserErrors.NotFound(request.Id));
        }

        var response = new GetUserByIdResponse(user.Id, user.Email);

        return response;
    }
}
