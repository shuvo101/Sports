using Microsoft.EntityFrameworkCore;
using IMTestProject.Common.Const;
using IMTestProject.Common.Extension_Methods;
using IMTestProject.Common.Entity;

namespace IMTestProject.Common
{
    // TODO: please experiment to inherit this one from DbContextWithTriggers, in case of success you can remove the 4 SaveChanges for the trigger
    public partial class IMTestProjectWriteOnlyContext : DbContext
    {
        public virtual DbSet<Continent> Continents { get; set; }
        public virtual DbSet<MainTable> MainTables { get; set; }
        public virtual DbSet<TableConfiguration> TableConfigurations { get; set; }
        public virtual DbSet<AdditionalInformation> AdditionalInformationDatas { get; set; }

        public IMTestProjectWriteOnlyContext () { }

        public IMTestProjectWriteOnlyContext (DbContextOptions<IMTestProjectWriteOnlyContext> options)
            : base (options) { }

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
            
            modelBuilder.ConfigureContinents ();
           
            OnModelCreatingPartial (modelBuilder);
        }

        partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
    }
}