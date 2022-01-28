using System;

namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public string Partition { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}