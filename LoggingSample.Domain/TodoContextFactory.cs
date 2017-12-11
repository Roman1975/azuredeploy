using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace LoggingSample.Domain
{
    /// <summary>
    /// to read https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
    /// </summary>
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var connectionString = @"Data Source=App_data/LoggingSample.db;";

            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlite(connectionString, b=>b.MigrationsAssembly("LoggingSample.Migrations"));

            return new TodoContext(optionsBuilder.Options);
        }
    }
}