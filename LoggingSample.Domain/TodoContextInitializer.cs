using System;
using System.Linq;
using LoggingSample.Domain.Model;


namespace LoggingSample.Domain
{
    public static class TodoContextInitializer
    {
        public static void Seed(TodoContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Todos.Any())
            {
                // Seed Here
                var todos = new TodoItem[]{
                        new TodoItem { Title = "test 1", DateStart = DateTime.Now },
                        new TodoItem { Title = "test 2", DateStart = DateTime.Now },
                        new TodoItem { Title = "test 3", DateStart = DateTime.Now },
                    };

                foreach (TodoItem t in todos)
                {
                    context.Todos.Add(t);
                }
                context.SaveChanges();
            }
        }
    }
}