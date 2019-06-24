using System;

namespace CarAdvertiser.DTO.Interfaces
{
    public interface IAuditableEntity : IBaseEntity
    {
        bool IsDeleted { get; set; }
        DateTime CreatedDate { get; set; }
        string CreateUser { get; set; }
        DateTime? LastModificationDate { get; set; }
        string LastModificationUser { get; set; }
    }
}
