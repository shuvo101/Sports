using Microsoft.EntityFrameworkCore;
using IMTestProject.Common.Const;
using IMTestProject.Common.Extension_Methods;
using IMTestProject.Common.Entity;

namespace IMTestProject.Common
{
    public partial class IMTestProjectReadOnlyContext : DbContext
    {
        public virtual DbSet<Continent> Continents { get; set; }
        public virtual DbSet<MainTable> MainTables { get; set; }
        public virtual DbSet<TableConfiguration> TableConfigurations { get; set; }
        public virtual DbSet<AdditionalInformation> AdditionalInformationDatas { get; set; }

        public IMTestProjectReadOnlyContext ()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public IMTestProjectReadOnlyContext (DbContextOptions<IMTestProjectReadOnlyContext> options)
            : base (options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            if (! optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer (DbContextConst.ConnectionString);
            }
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation ("ProductVersion", "2.2.0-rtm-35687");

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ConfigureContinents();
            
            OnModelCreatingPartial (modelBuilder);
        }

        partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
    }
}