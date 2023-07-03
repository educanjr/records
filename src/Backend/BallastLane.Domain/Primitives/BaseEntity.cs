
namespace BallastLane.Domain.Primitives;
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; private init; }

	protected BaseEntity()
	{
	}

	protected BaseEntity(Guid id) => this.Id = id;

    public bool Equals(BaseEntity? other)
    {
        if(other is null) return false;

        if(other.GetType() != GetType()) return false;

        return other.Id == Id;
    }

    public static bool operator ==(BaseEntity? left, BaseEntity? right) => left is not null && left.Equals(right);

    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not BaseEntity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode() * 31;
}
