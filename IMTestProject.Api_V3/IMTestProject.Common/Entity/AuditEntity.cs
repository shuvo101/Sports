using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace IMTestProject.Common
{
    public abstract class AuditEntity<TPk> : BaseEntity<TPk>
    {
        [Column ("Created At", Order = 251, TypeName = "datetime2 (3)")]
        public DateTime CreatedAt { get; set; }

        [Column ("Updated At", Order = 252, TypeName = "datetime2 (3)")]
        public DateTime? UpdatedAt { get; set; }

        [Column ("Deleted At", Order = 253, TypeName = "datetime2 (3)")]
        public DateTime? DeletedAt { get; set; }
        [Column("Is Deleted", Order = 254, TypeName = "bit")]
        public bool IsDeleted { get; set; }

        
        public void EntityCreated()
        {
            //this.IsActive = true;
            this.CreatedAt = DateTime.UtcNow;
            this.IsDeleted = false;
            
        }
        public void EntityUpdated ()
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void EntityRemoved()
        {
            this.DeletedAt = DateTime.UtcNow;
            this.IsDeleted = true;
        }
    }
}