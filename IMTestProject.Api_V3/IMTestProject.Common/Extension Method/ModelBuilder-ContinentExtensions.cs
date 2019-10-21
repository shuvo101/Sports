using Microsoft.EntityFrameworkCore;

namespace IMTestProject.Common.Extension_Methods
{
    public static partial class ModelBuilderExtensions
    {
        public static void ConfigureContinents (this ModelBuilder builder)
        {
            builder.Entity<Continent> (entity =>
            {
                entity.ToTable ("Continent", "world");
                entity.HasIndex (e => e.Code)
                    .HasName ("IX_Continent_Index")
                    .IsUnique ();
               
                entity.Property (e => e.Code)
                    .HasColumnName ("Code")
                    .HasColumnType ("nvarchar(20)")
                    .HasMaxLength(20)
                    .IsRequired ();
              
                entity.Property (e => e.Name)
                    .HasColumnName ("Name")
                    .HasColumnType ("nvarchar(60)")          
                    .HasMaxLength(60)
                    .IsRequired ();
            });

            builder.Entity<Continent> ()
                .HasQueryFilter (it => it.IsActive);
        }
    }
}