using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using People365ToDoList.API.Models;
using People365ToDoList.Models;

namespace People365ToDoList.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDToDoListController : ControllerBase
    {

        private static List<ToDoList> _ToDoLists = new List<ToDoList>();
        private static int _nextId = 1;
        private readonly ILogger<CRUDToDoListController> _logger;

        public CRUDToDoListController(ILogger<CRUDToDoListController> logger)
        {
            _logger = logger;
        }
        [NonAction]
        public void OnGet()
        {
            _logger.LogInformation("CRUD To Do List Controller loaded successfully at {dateTime}",
            DateTime.UtcNow.ToLongTimeString());
        }

        [HttpGet]
        public IActionResult GetToDoList() // This function returns the entire to do list
        {
            try
            {
                _logger.LogInformation("GetToDoList function returned the to do list successfully at {dateTime}",
                DateTime.UtcNow.ToLongTimeString());

                return Ok(_ToDoLists.Where(t => t.isDeleted == false).OrderBy(t => t.Priority));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetToDoList function catched an error {errorMessage} at {dateTime}",
                ex.Message, DateTime.UtcNow.ToLongTimeString());

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetToDoListById(int id) // This function returns a to do task per id
        {
            try
            {
                var toDoList = _ToDoLists.FirstOrDefault(t => t.id == id && t.isDeleted == false);
                if (toDoList == null)
                {
                    _logger.LogInformation("GetToDoListById function returned null as the id {id} is not found at {dateTime}",
                    id, DateTime.UtcNow.ToLongTimeString());

                    return NotFound();
                }

                _logger.LogInformation("GetToDoListById function returned the to do task successfully having id {id} at {dateTime}",
               id, DateTime.UtcNow.ToLongTimeString());

                return Ok(toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetToDoListById function catched an error {errorMessage} at {dateTime}",
                   ex.Message, DateTime.UtcNow.ToLongTimeString());

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddTaskToDoList([FromBody] ToDoListAddEditModel addToDoList) // This function adds a new task to the to do list
        {
            try
            {
                ToDoList toDoList = new ToDoList { 
                    id = _nextId++,
                    Description = addToDoList.Description,
                    Title = addToDoList.Title,
                    Priority = addToDoList.Priority,
                    isDeleted = false,
                };

                _ToDoLists.Add(toDoList);

                _logger.LogInformation("A new task has been added to the to do list at {dateTime}",
              DateTime.UtcNow.ToLongTimeString());

                return CreatedAtAction(nameof(AddTaskToDoList), new { id = toDoList.id }, toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddTaskToDoList function catched an error {errorMessage} at {dateTime}",
                   ex.Message, DateTime.UtcNow.ToLongTimeString());

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTaskToDoList(int id, [FromBody] ToDoListAddEditModel updatedToDoList) // This function updates a task in the to do list with specific id
        {
            try
            {
                var toDoList = _ToDoLists.FirstOrDefault(t => t.id == id && t.isDeleted == false);
                if (toDoList == null)
                {

                    _logger.LogInformation("UpdateTaskToDoList function returned null as the id {id} is not found at {dateTime}",
                        id, DateTime.UtcNow.ToLongTimeString());

                    return NotFound();
                }
                toDoList.Title = updatedToDoList.Title;
                toDoList.Description = updatedToDoList.Description;
                toDoList.Priority = updatedToDoList.Priority;;

                _logger.LogInformation("Record with id {id} is successfully updated at {dateTime}",
              id, DateTime.UtcNow.ToLongTimeString());

                return Ok("To do list task updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskToDoList function catched an error {errorMessage} at {dateTime}",
                   ex.Message, DateTime.UtcNow.ToLongTimeString());

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTaskToDoList(int id) // This function deletes a task from the to do list
        {
            try
            {
                var toDoList = _ToDoLists.FirstOrDefault(t => t.id == id && t.isDeleted == false);
                if (toDoList == null)
                {

                    _logger.LogInformation("DeleteTaskToDoList function returned null as the id {id} is not found at {dateTime}",
                       id, DateTime.UtcNow.ToLongTimeString());

                    return NotFound();
                }
                toDoList.isDeleted = true;

                _logger.LogInformation("Task with the id {id} has been deleted successfully at {dateTime}",
               id, DateTime.UtcNow.ToLongTimeString());

                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteTaskToDoList function catched an error {errorMessage} at {dateTime}",
                   ex.Message, DateTime.UtcNow.ToLongTimeString());

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
