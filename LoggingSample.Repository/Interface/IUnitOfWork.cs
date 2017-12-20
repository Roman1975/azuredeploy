using LoggingSample.Domain;
using LoggingSample.Domain.Model;

namespace LoggingSample.Repository.Interface
{
    public interface IUnitOfWork   
    {  
        IGenericRepository<TodoItem> TodoRepository { get; }
  
        int Save();  
    }  
}