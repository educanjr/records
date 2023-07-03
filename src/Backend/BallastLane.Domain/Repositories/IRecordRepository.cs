
using BallastLane.Domain.Entities;

namespace BallastLane.Domain.Repositories;

public interface IRecordRepository
{
    Task Add(Record record);

    Task Update(Record record);

    Task Delete(Guid id, Guid creatorId);

    Task<Record> Get(Guid id);

    Task<IEnumerable<Record>> GetByCreator(Guid id);

    Task<IEnumerable<Record>> GetAll();
}
