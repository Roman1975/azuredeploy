using System.Linq;
using LoggingSample.Repository.Interface;
using LoggingSample.Domain;

namespace LoggingSample.Repository
{  
    public class GenericRepository<T> : IGenericRepository<T> where T : class  
    {  
        protected TodoContext _context;  
  
        public GenericRepository(TodoContext context)  
        {  
            _context = context;  
        }  
  
        public IQueryable<T> GetAll()  
        {  
            IQueryable<T> query = _context.Set<T>();  
            return query;  
        }  
  
        public void Insert(T entity)  
        {  
            _context.Set<T>().Add(entity);  
        }  
  
        public void Update(T entity)  
        {  
            _context.Set<T>().Attach(entity);  
        }  
  
        public void Delete(T entity)  
        {  
            _context.Set<T>().Remove(entity);  
        }  
    }  
}  