using BallastLane.Domain.Common;
using BallastLane.Domain.Errors;
using BallastLane.Domain.Primitives;

namespace BallastLane.Domain.Entities;
public class User : BaseEntity, IAuditableEntity
{
    public User(Guid id, string email, string firstName, string lastName, string passwordHash)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
    }

    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public string PasswordHash { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public static DomainResult<User> Create(Guid id, string email, string firstName, string lastName, string passwordHash)
    {
        var emailResult = DomainResult.Ensure(
            email,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.EmailEmpty),
            (x => x.Length <= UserErrors.EmailMaxLength, UserErrors.EmailTooLong),
            (x => x.Split('@').Length == 2, UserErrors.EmailIncorrectFormat));

        var firstNameResult = DomainResult.Ensure(
            firstName,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.FirstNameEmpty),
            (x => x.Length <= UserErrors.FirstNameMaxLength, UserErrors.FirstNameTooLong));

        var lastNameResult = DomainResult.Ensure(
            lastName,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.LastNameEmpty),
            (x => x.Length <= UserErrors.LastNameMaxLength, UserErrors.LastNameTooLong));

        var passwordResult = DomainResult.Ensure(
            passwordHash,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.PasswordEmpty));

        if(emailResult.IsFailure || firstNameResult.IsFailure || lastNameResult.IsFailure || passwordResult.IsFailure)
        {
            var errors = new List<Error>();
            errors.AddRange(emailResult.Errors.Where(i => i != Error.None));
            errors.AddRange(firstNameResult.Errors.Where(i => i != Error.None));
            errors.AddRange(lastNameResult.Errors.Where(i => i != Error.None));
            errors.AddRange(passwordResult.Errors.Where(i => i != Error.None));

            return DomainResult.Failure<User>(errors.ToArray());
        }

        var user = new User(id, email, firstName, lastName, passwordHash);
        return user;
    }

    public DomainResult ChangeName(string firstName, string lastName) 
    {
        var firstNameResult = DomainResult.Ensure(
            firstName,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.FirstNameEmpty),
            (x => x.Length <= UserErrors.FirstNameMaxLength, UserErrors.FirstNameTooLong));

        var lastNameResult = DomainResult.Ensure(
            lastName,
            (x => !string.IsNullOrWhiteSpace(x), UserErrors.LastNameEmpty),
            (x => x.Length <= UserErrors.LastNameMaxLength, UserErrors.LastNameTooLong));

        if (firstNameResult.IsFailure || lastNameResult.IsFailure)
        {
            var errors = new List<Error>();
            errors.AddRange(firstNameResult.ValidErrors);
            errors.AddRange(lastNameResult.ValidErrors);

            return DomainResult.Failure(errors.ToArray());
        }

        FirstName = firstName;
        LastName = lastName;

        return DomainResult.Success();
    }
}
