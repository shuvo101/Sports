using IMTestProject.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace IMTestProject.Common.Extension_Methods
{
    public static partial class ModelBuilderExtensions
    {
        public static void ConfigureMainTables(this ModelBuilder builder)
        {
            builder.Entity<MainTable> (entity =>
            {
                entity.ToTable ("MainTable", "world");
                entity.HasIndex (e => e.TableName)
                    .HasName ("IX_MainTable_Index")
                    .IsUnique ();
               
                entity.Property (e => e.TableName)
                    .HasColumnName ("TableName")
                    .HasColumnType ("nvarchar(50)")
                    .HasMaxLength(20)
                    .IsRequired ();
              
                entity.Property (e => e.TableCode)
                    .HasColumnName ("TableCode")
                    .HasColumnType ("nvarchar(60)")          
                    .HasMaxLength(60)
                    .IsRequired ();
            });

            builder.Entity<MainTable> ()
                .HasQueryFilter (it => it.IsActive);
        }
    }
}