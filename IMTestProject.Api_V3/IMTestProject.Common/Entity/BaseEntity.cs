using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IMTestProject.Common
{
    public abstract class BaseEntity<TPk> : IEntity<TPk>
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        [Column ("Id", Order = 0)]
        public TPk Id { get; set; }

        [Column ("Is Active", Order = 255, TypeName = "bit")]
        public bool IsActive { get; set; }

        public void EntityActivated()
        {
            this.IsActive = true;
        }
        public void EntityInactivated()
        {
            this.IsActive = false;
        }

        
        
        //static BaseEntity ()
        //{
        //    Triggers<BaseEntity<TPk>>.Inserting += entry =>
        //    {
        //        entry.Entity.CreatedAt = DateTime.UtcNow;
        //        entry.Entity.IsDeleted = false;
        //        entry.Entity.IsActive = true;
        //    };

        //    Triggers<BaseEntity<TPk>>.Updating += entry =>
        //        entry.Entity.UpdatedAt = DateTime.UtcNow;

        //    Triggers<BaseEntity<TPk>>.Deleting += entry =>
        //    {
        //        entry.Entity.DeletedAt = DateTime.UtcNow;
        //        entry.Entity.IsDeleted = true;
        //        entry.Cancel = true;
        //    };
        //}
    }
}