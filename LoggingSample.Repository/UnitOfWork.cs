using LoggingSample.Repository.Interface;
using LoggingSample.Domain;
using System;
using LoggingSample.Domain.Model;

namespace LoggingSample.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;

        private IGenericRepository<TodoItem> _todoRepo;

        public UnitOfWork(TodoContext context)
        {
            _context = context;
            //_context.Configuration.LazyLoadingEnabled = false;  
        }

        public IGenericRepository<TodoItem> TodoRepository
        {
            get
            {
                if (_todoRepo == null)
                    _todoRepo = new GenericRepository<TodoItem>(_context);

                return _todoRepo;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

    }
}