using IMTestProject.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace IMTestProject.Common.Extension_Methods
{
    public static partial class ModelBuilderExtensions
    {
        public static void ConfigureTableConfigurations(this ModelBuilder builder)
        {
            builder.Entity<TableConfiguration> (entity =>
            {
                entity.ToTable ("TableConfiguration", "world");
                entity.HasIndex (e => e.Name)
                    .HasName ("IX_TableConfiguration_Index")
                    .IsUnique ();

                entity.Property(e => e.Name)
                     .HasColumnName("Name")
                     .HasColumnType("nvarchar(50)")
                     .HasMaxLength(20)
                     .IsRequired();

                entity.Property (e => e.DisplayName)
                    .HasColumnName ("DisplayName")
                    .HasColumnType ("nvarchar(100)")
                    .HasMaxLength(20)
                    .IsRequired ();

                entity.Property(e => e.ControlType)
                    .HasColumnName("ControlType")
                    .HasColumnType("nvarchar(60)")
                    .HasMaxLength(60)
                    .IsRequired ();

                entity.Property(e => e.DataType)
                   .HasColumnName("DataType")
                   .HasColumnType("nvarchar(60)")
                   .HasMaxLength(60)
                   .IsRequired();

                entity.Property(e => e.ReferanceTable)
                   .HasColumnName("ReferanceTable")
                   .HasColumnType("nvarchar(60)")
                   .HasMaxLength(60);
                //.IsRequired();

                entity.Property(e => e.TextField)
                   .HasColumnName("TextField")
                   .HasColumnType("nvarchar(60)")
                   .HasMaxLength(60);
                //.IsRequired();

                entity.Property(e => e.ValueField)
                   .HasColumnName("ValueField")
                   .HasColumnType("nvarchar(60)")
                   .HasMaxLength(60);
                   //.IsRequired();
            });

            builder.Entity<TableConfiguration> ()
                .HasQueryFilter (it => it.IsActive);
        }
    }
}