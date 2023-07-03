
using AutoMapper;
using BallastLane.Domain.Repositories;
using BallastLane.Domain.Entities;
using BallastLane.Persistence.Abstractions;
using BallastLane.Persistence.DTOs;

namespace BallastLane.Persistence.Repositories;

public class RecordRepository : BaseRepository<RecordDTO>, IRecordRepository
{
    private readonly IMapper _mapper;
    public RecordRepository(IAppDbContext context, IMapper mapper)
        : base(context)
    {
        _mapper = mapper;
    }

    public async Task Add(Record record)
    {
        var recordDTO = _mapper.Map<RecordDTO>(record);
        var query = """
            INSERT INTO Records (Id, Title, Description, CreatorId)
            VALUES (@Id, @Title, @Description, @CreatorId)
        """;

        await ExecuteAsync(query, recordDTO);
    }

    public async Task Update(Record record)
    {
        var recordDTO = _mapper.Map<RecordDTO>(record);
        var query = """
            UPDATE Records 
            SET Title = @Title,
                Description = @Description
            WHERE Id = @Id AND CreatorId = @CreatorId
        """;

        await ExecuteAsync(query, recordDTO);
    }

    public async Task Delete(Guid id, Guid creatorId)
    {
        var query = """
            DELETE FROM Records
            WHERE Id = @id AND CreatorId = @creatorId
        """;

        await ExecuteAsync(query, new { id = id.ToString(), creatorId = creatorId.ToString() });
    }

    public async Task<Record> Get(Guid id)
    {
        var query = """
            SELECT R.Id, R.Title, R.Description, U.Id as CreatorId, U.Email as CreatorEmail, U.FirstName as CreatorFirstName, U.LastName as CreatorLastName 
            FROM Records as R
            INNER JOIN Users as U
                ON R.CreatorId = U.Id
            WHERE R.Id = @id
        """;

        var recordDTO = await ExecuteQuerySingleOrDefaultAsync(query, new { id = id.ToString() });
        return _mapper.Map<Record>(recordDTO);
    }

    public async Task<IEnumerable<Record>> GetByCreator(Guid id)
    {
        var query = """
            SELECT R.Id, R.Title, R.Description, U.Id as CreatorId, U.Email as CreatorEmail, U.FirstName as CreatorFirstName, U.LastName as CreatorLastName 
            FROM Records as R
            INNER JOIN Users as U
                ON R.CreatorId = U.Id 
            WHERE R.CreatorId = @id
        """;

        var recordsDTO = await ExecuteQueryAsync(query, new { id = id.ToString() });
        return recordsDTO.Select(i => _mapper.Map<Record>(i)).ToList();
    }

    public async Task<IEnumerable<Record>> GetAll()
    {
        var query = """
            SELECT R.Id, R.Title, R.Description, U.Id as CreatorId, U.Email as CreatorEmail, U.FirstName as CreatorFirstName, U.LastName as CreatorLastName 
            FROM Records as R
            INNER JOIN Users as U
                ON R.CreatorId = U.Id
        """;

        var recordsDTO = await ExecuteQueryAsync(query);
        return recordsDTO.Select(i => _mapper.Map<Record>(i)).ToList();
    }
}
