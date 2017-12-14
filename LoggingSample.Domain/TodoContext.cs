using Microsoft.EntityFrameworkCore;  
using LoggingSample.Domain.Model;

namespace LoggingSample.Domain
{  
    public class TodoContext : DbContext    
    {   
        public TodoContext(DbContextOptions<TodoContext> options)  
            : base(options)  
        { }  
  
        public DbSet<TodoItem> Todos { get; set; }  
    }  
}   