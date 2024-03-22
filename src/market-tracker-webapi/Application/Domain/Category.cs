namespace market_tracker_webapi.Application.Domain;

public record Category(int Id, string Name, int? ParentId = null)
{
    public Category(string name, int? parentId = null) : this(0, name, parentId)
    {
        if (name.Length < 3)
        {
            throw new DomainException(name);
        }
    }
}