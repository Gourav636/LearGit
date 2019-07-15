using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext todoContext;

        public TodoController(TodoContext todoContext)
        {
            this.todoContext = todoContext;

            if(todoContext.TodoItems.Count() == 0)
            {
                todoContext.TodoItems.Add(new TodoItem() { Name = "Test" });
                todoContext.SaveChanges();
            }
        }

        //Get :  api/Todo/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItems = await todoContext.TodoItems.FindAsync(id);

            if(todoItems == null)
            {
                return NotFound();
            }

            return todoItems;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            todoContext.TodoItems.Add(item);
            await todoContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            todoContext.Entry(item).State = EntityState.Modified;
            await todoContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItem(long id)
        {
            var result = await todoContext.TodoItems.FindAsync(id);

            if(result == null)
            {
                return NotFound();
            }
            todoContext.TodoItems.Remove(result);
            await todoContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
