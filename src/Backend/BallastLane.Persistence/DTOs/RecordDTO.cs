
namespace BallastLane.Persistence.DTOs;

public class RecordDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string CreatorId { get; set; }
    public string CreatorEmail { get; set; }
    public string CreatorFirstName { get; set; }
    public string CreatorLastName { get; set; }
    public string CreatorPassword { get; set; }
}
