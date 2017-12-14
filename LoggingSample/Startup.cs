using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LoggingSample.Domain;
using LoggingSample.Repository;
using LoggingSample.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace LoggingSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            var connectionString = Configuration["ConnectionStrings:SQLAzure"];
            
            services.AddEntityFrameworkSqlServer()
                .AddDbContextPool<TodoContext>(     // high performance https://docs.microsoft.com/en-us/ef/core/what-is-new/
                    optionsAction => optionsAction.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly("LoggingSample.Migrations"))); //https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects
            //
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            // Add framework services.


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var context = services.GetRequiredService<TodoContext>();
                //var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    try
                    {
                        TodoContextInitializer.Seed(context);
                        logger.LogInformation("Database seeding complete");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
                else
                {
                    try
                    {
                        context.Database.Migrate();
                        logger.LogInformation("Database migration complete");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while migrating the database.");
                    }
                }
            }

            // read more here https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseMvc();
        }
    }
}
