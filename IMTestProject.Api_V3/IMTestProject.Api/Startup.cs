using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Business.Implementation;
using IMTestProject.Business.Interface;
using IMTestProject.Common;
using IMTestProject.Common.Const;
using IMTestProject.DAL.Implementation;
using IMTestProject.DAL.Interface;
using IMTestProject.Service.Implementation;
using IMTestProject.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace IMTestProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IMTestProjectWriteOnlyContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString(DbContextConst.ConnectionString),
                   b => b.MigrationsAssembly(typeof(Startup).Assembly.FullName)
               ), ServiceLifetime.Scoped
            );

            services.AddDbContext<IMTestProjectReadOnlyContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString(DbContextConst.ConnectionString),
                   b => b.MigrationsAssembly(typeof(Startup).Assembly.FullName)
               ), ServiceLifetime.Scoped
            );

            services.AddTransient<IContinentService, ContinentService>();
            services.AddScoped<IContinentBusiness, ContinentBusiness>();
            services.AddScoped<IContinentRepository, ContinentRepository>();

            services.AddTransient<IMainTableService, MainTableService>();
            services.AddScoped<IMainTableBusiness, MainTableBusiness>();
            services.AddScoped<IMainTableRepository, MainTableRepository>();

            services.AddTransient<ITableConfigurationService, TableConfigurationService>();
            services.AddScoped<ITableConfigurationBusiness, TableConfigurationBusiness>();
            services.AddScoped<ITableConfigurationRepository, TableConfigurationRepository>();

            services.AddTransient<IAdditionalInformationService, AdditionalInformationService>();
            services.AddScoped<IAdditionalInformationBusiness, AdditionalInformationBusiness>();
            services.AddScoped<IAdditionalInformationRepository, AdditionalInformationRepository>();

            services.AddTransient<IIMTestProjectUnitOfWork, IMTestProjectUnitOfWork>();


            // TODO: check if this is needed: AddControllers....appears to be new in Core 3
            //services.AddControllers();



            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //.AddNewtonsoftJson(options =>
            //{
            //    if (options.SerializerSettings != null)
            //    {
            //        options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            //    }
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "IM TEST PROJECT API ", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMTestProjectWriteOnlyContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (!dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }
                //dbContext.Database.EnsureCreated ();  //TODO: remove this after finishing development
            }
            else
            {
                if (!dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }
            }

            app.UseMvc();

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IM TEST PROJECT API V1");
            });

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

        }
    }
}
