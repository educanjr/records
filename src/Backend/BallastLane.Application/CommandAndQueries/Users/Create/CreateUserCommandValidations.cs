
using BallastLane.Domain.Errors;
using FluentValidation;

namespace BallastLane.Application.CommandAndQueries.Users.Create;

internal class CreateUserCommandValidations : AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidations()
	{
		RuleFor(x => x.FirstName).NotEmpty().MaximumLength(UserErrors.FirstNameMaxLength);
		RuleFor(x => x.LastName).NotEmpty().MaximumLength(UserErrors.LastNameMaxLength);
		RuleFor(x => x.Password).NotEmpty().MaximumLength(UserErrors.PasswordMaxLength).MinimumLength(UserErrors.PasswordMinLength);
	}
}
