namespace CarAdvertiser.DTO.Interfaces
{
    public interface IValueEntity : IAuditableEntity
    {
        string Value { get; set; }
    }
}