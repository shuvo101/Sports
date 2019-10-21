using IMTestProject.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace IMTestProject.Common.Extension_Methods
{
    public static partial class ModelBuilderExtensions
    {
        public static void ConfigureTableMappingDataExtensions(this ModelBuilder builder)
        {
            //builder.Entity<AdditionalInformation>()
            //    .HasOne<Continent>(s => s.Continent)
            //    .WithMany(ta => ta.TableMappingDatas)
            //    .HasForeignKey(u => u.ContinentId)
            //    .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<AdditionalInformation>(entity =>
           {
               entity.ToTable("AdditionalInformation", "world");
               entity.HasIndex(e => e.Value)
                   .HasName("IX_AdditionalInformation_Index")
                   .IsUnique();

               entity.Property(e => e.Value)
                    .HasColumnName("Value")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(20)
                    .IsRequired();

               entity.Property(e => e.Code)
                   .HasColumnName("Code")
                   .HasColumnType("nvarchar(100)")
                   .HasMaxLength(20)
                   .IsRequired();
               
               entity.HasOne(d => d.Continent)
                   .WithMany(p => p.TableMappingDatas)
                   .HasForeignKey(d => d.ContinentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Continent_AdditionalInformation");
               entity.HasOne(d => d.TableConfiguration)
                   .WithMany(p => p.AdditionalInformations)
                   .HasForeignKey(d => d.TableConfigurationId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_TableConfiguration_AdditionalInformation");
           });

            builder.Entity<AdditionalInformation>()
                .HasQueryFilter(it => it.IsActive);
        }
    }
}