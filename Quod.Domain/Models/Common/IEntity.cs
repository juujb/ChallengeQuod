namespace Quod.Domain
{
    public interface IEntity
    {
        string Id { get; set; }
        bool Deleted { get;set; }
    }
}
