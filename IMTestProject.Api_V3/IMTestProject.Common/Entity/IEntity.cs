using System;

namespace IMTestProject.Common
{
    public interface IEntity<TPk>
    {
        TPk Id { get; set; }
        //DateTime CreatedAt { get; set; }
        //DateTime? UpdatedAt { get; set; }
        //DateTime? DeletedAt { get; set; }
        //bool IsDeleted { get; set; }
        bool IsActive { get; set; }
        void EntityActivated();
        void EntityInactivated();
        //void EntityCreated();
        //void EntityUpdated();
        //void EntityRemoved();
    }
}