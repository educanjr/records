
using BallastLane.Domain.Entities;
using AutoMapper;
using BallastLane.Persistence.DTOs;

namespace BallastLane.Persistence.Mappers;

internal class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        // User -> UserDTO
        CreateMap<User, UserDTO>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .DisableCtorValidation()
            .ConvertUsing<UserToUserDTO>();

        // UserDTO -> User
        CreateMap<UserDTO, User>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .DisableCtorValidation()
            .ConvertUsing<UserDTOToUser>();

        // Record -> RecordDTO
        CreateMap<Record, RecordDTO>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .DisableCtorValidation()
            .ForMember(
                dst => dst.Id,
                opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(
                dst => dst.CreatorId,
                opt => opt.MapFrom(src => src.Creator.Id.ToString()));

        // RecordDTO -> Record
        CreateMap<RecordDTO, Record>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .DisableCtorValidation()
            .ConvertUsing<RecordDTOToRecord>();
    }
}

public class UserToUserDTO : ITypeConverter<User, UserDTO>
{
    public UserDTO Convert(User src, UserDTO dst, ResolutionContext context)
    {
        var id = src.Id.ToString();
        var firstName = src.FirstName;
        var lastName = src.LastName;
        var password = src.PasswordHash;
        var email = src.Email;

        var result = new UserDTO
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = password,
            Email = email
        };

        return result;
    }
}

public class UserDTOToUser : ITypeConverter<UserDTO, User>
{
    public User Convert(UserDTO src, User dst, ResolutionContext context)
    {
        var id = Guid.Parse(src.Id);
        var firstName = src.FirstName;
        var lastName = src.LastName;
        var password = src.PasswordHash;
        var email = src.Email;

        var result = new User(id, email, firstName, lastName, password);
        return result;
    }
}

public class RecordDTOToRecord : ITypeConverter<RecordDTO, Record>
{
    public Record Convert(RecordDTO src, Record dst, ResolutionContext context)
    {
        var cretorId = Guid.Parse(src.CreatorId);
        var firstName = src.CreatorFirstName;
        var lastName = src.CreatorLastName;
        var email = src.CreatorEmail;

        var id = Guid.Parse(src.Id);
        var title = src.Title;
        var description = src.Description;

        User creator = new(cretorId, email, firstName, lastName, string.Empty);

        Record result = new(id, creator, title, description); 
        return result;
    }
}
