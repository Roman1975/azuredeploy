using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LoggingSample.Domain
{
    /// <summary>
    /// to read https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
    /// </summary>
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connectionString = configuration["ConnectionStrings:SQLAzure"];

            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer(connectionString, 
                b=>b.MigrationsAssembly("LoggingSample.Migrations"));

            return new TodoContext(optionsBuilder.Options);
        }
    }
}