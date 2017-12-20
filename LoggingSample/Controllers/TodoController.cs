using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoggingSample.LoggingStaff;
using LoggingSample.Domain.Model;
using LoggingSample.Repository.Interface;

namespace LoggingSample.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly ILogger _logger;
        public TodoController(ILogger<TodoController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _work = unitOfWork;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            _logger.Get();
            return Ok(_work.TodoRepository.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}", Name ="GetTodo")]
        public IActionResult Get(int id)
        {
            TodoItem item = null;

            if (IsItemExists(id))
            {
                item = LoadTodoItem(id);
                _logger.GetItem(id);

                return new ObjectResult(item);
            }
            return NotFound();
        }

        private TodoItem LoadTodoItem(int id, bool withTracking = true)
        {
            //if(!withTracking)
            //    return _work.TodoRepository.GetAll().AsNoTracking().Where(x => x.Id == id).First();
            return _work.TodoRepository.GetAll().Where(x => x.Id == id).First();
        }

        private bool IsItemExists(int id)
        {
            var isItemExists = _work.TodoRepository.GetAll().Any(a=>a.Id == id);

            if(!isItemExists)
            {
                _logger.NotFound(id);
            }
            return isItemExists;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]TodoItem item)
        {
            if (item == null)
                return BadRequest();

            //item.Id =_data.Any()? (_data.GroupBy(x => x.Id).Max(m => m.Key) + 1):1;
            _work.TodoRepository.Insert(item);
            _work.Save();
            _logger.PostItem(item.Title);

            var result = CreatedAtRoute("GetTodo", new { id = item.Id }, item); 
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TodoItem item)
        {
            if (item == null || id!= item.Id)
            {
                return BadRequest();
            }
            
            using(_logger.UpdateWithScope(id))
            {
                if(IsItemExists(id))
                {
                    var oldItem = LoadTodoItem(id, false);
                    oldItem.DateFinish = item.DateFinish;
                    oldItem.DateStart = item.DateStart;
                    oldItem.IsCompleted = item.IsCompleted;
                    oldItem.Title = item.Title;

                    _work.TodoRepository.Update(oldItem);
                    _work.Save();

                    _logger.PutItem(id, oldItem.Title, item.Title);

                    return new NoContentResult();
                }
            }
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try{
                var itemToDelete = LoadTodoItem(id);
                _logger.DeleteItem(itemToDelete.Title, id);
                _work.TodoRepository.Delete(itemToDelete);
                _work.Save();
            }
            catch(Exception ex){
                _logger.DeleteItemFailed(id, ex);
            }
            return Ok();
        }

                // DELETE api/values/5
        [HttpDelete()]
        public IActionResult Delete()
        {
            var count = _work.TodoRepository.GetAll().Count();
            
            using(_logger.DeleteAllScope(count))
            {
                while(_work.TodoRepository.GetAll().Any())
                {
                    var item = _work.TodoRepository.GetAll().First();
                    _work.TodoRepository.Delete(item);
                    _logger.DeleteItem(item.Title, item.Id);
                }
                _work.Save();
            }

            return Ok(count);
        }
    }
}
