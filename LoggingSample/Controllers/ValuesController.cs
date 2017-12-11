using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoggingSample.LoggingStaff;
using LoggingSample.Model;

namespace LoggingSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // simple static storage 
        private static IList<ItemModel> _data = new List<ItemModel>(){
            new ItemModel { Id = 1, Value = "value1" },
            new ItemModel { Id = 2, Value = "value2" },
            new ItemModel { Id = 3, Value = "value3" },
            new ItemModel { Id = 4, Value = "value4" },
            new ItemModel { Id = 5, Value = "value5" },
        };

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            _logger.Get();
            return Ok(_data);
        }

        // GET api/values/5
        [HttpGet("{id}", Name ="GetItem")]
        public IActionResult Get(int id)
        {
            ItemModel item = null;

            if (IsItemExists(id))
            {
                item = _data.Where(x=>x.Id==id).FirstOrDefault();
                _logger.GetItem(id);

                return new ObjectResult(item);
            }
            return NotFound();
        }

        private bool IsItemExists(int id)
        {
            var isItemExists = _data.Any(a=>a.Id == id);

            if(!isItemExists)
            {
                _logger.NotFound(id);
            }
            return isItemExists;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ItemModel item)
        {
            if (item == null || item.Value == null)
                return BadRequest();

            item.Id =_data.Any()? (_data.GroupBy(x => x.Id).Max(m => m.Key) + 1):1;
            _data.Add(item);
            _logger.PostItem(item.Value);

            var result = CreatedAtRoute("GetItem", new { id = item.Id }, item); 
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ItemModel item)
        {
            if (item == null || item.Value == null || id!= item.Id)
            {
                return BadRequest();
            }
            
            using(_logger.UpdateWithScope(id))
            {
                if(IsItemExists(id))
                {
                    var oldItemValue = _data[id].Value;
                    _data[id].Value = item.Value;
                    _logger.PutItem(id, oldItemValue, item.Value);
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
                _logger.DeleteItem(_data[id].Value, id);
                _data.RemoveAt(id);
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
            var count = _data.Count;
            
            using(_logger.DeleteAllScope(count))
            {
                while(_data.Any())
                {
                    var item = _data[0];
                    _data.RemoveAt(0);
                    _logger.DeleteItem(item.Value, item.Id);
                }
            }

            return Ok(count);
        }
    }
}
