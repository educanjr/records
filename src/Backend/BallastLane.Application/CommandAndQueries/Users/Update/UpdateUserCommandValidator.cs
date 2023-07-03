
using BallastLane.Domain.Errors;
using FluentValidation;

namespace BallastLane.Application.CommandAndQueries.Users.Update;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(UserErrors.FirstNameMaxLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(UserErrors.LastNameMaxLength);
    }
}
