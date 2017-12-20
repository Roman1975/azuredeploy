using System.Linq;

namespace LoggingSample.Repository.Interface
{
    public interface IGenericRepository<T> where T : class  
    {  
        IQueryable<T> GetAll();  
  
        void Insert(T entity);  
  
        void Update(T entity);  
  
        void Delete(T entity);  
    }  
}