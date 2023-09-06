using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using People365ToDoList.API.Models;
using People365ToDoList.Controllers;
using People365ToDoList.Models;

namespace CRUDToDoList.Test
{
    public class CRUDToDoListControllerTests
    {

        [Fact]
        public void GetToDoList_ReturnsOkResultWithToDoLists()
        {
            // Arrange
            var controller = CreateController();
            
            // Act
            var result = controller.GetToDoList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var toDoLists = Assert.IsAssignableFrom<IEnumerable<ToDoList>>(okResult.Value);
            Assert.True(toDoLists.Any()); // Assuming that the list has data
        }

        [Fact]
        public void GetToDoListById_ReturnsOkResultWithToDoList()
        {
            // Arrange
            var id = 1;
            var controller = CreateController();

            // Act
            var result = controller.GetToDoListById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var toDoList = Assert.IsType<ToDoList>(okResult.Value);
            Assert.Equal(id, toDoList.id);
        }

        [Fact]
        public void GetToDoListById_ReturnsNotFoundResult()
        {
            // Arrange
            var id = 999; // Assuming this ID doesn't exist
            var controller = CreateController();

            // Act
            var result = controller.GetToDoListById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddTaskToDoList_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var toDoList = new ToDoListAddEditModel
            {
                Title = "Brush teeth",
                Description = "Brush my teeth in the morning",
                Priority = 1,
            };
            var controller = CreateController();

            // Act
            var result = controller.AddTaskToDoList(toDoList);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdToDoList = Assert.IsType<ToDoList>(createdAtActionResult.Value);
            Assert.Equal(1, createdToDoList.id);
        }

        [Fact]
        public void UpdateTaskToDoList_ReturnsNoContentResult()
        {
            // Arrange
            var id = 1; // Assuming this ID exists
            var updatedToDoList = new ToDoListAddEditModel
            {
                Title = "Brush teeth",
                Description = "Brush my teeth in the morning",
                Priority = 1,
            };
            var controller = CreateController();

            // Act
            var result = controller.UpdateTaskToDoList(id, updatedToDoList);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateTaskToDoList_ReturnsNotFoundResult()
        {
            // Arrange
            var id = 999; // Assuming this ID doesn't exist
            var updatedToDoList = new ToDoListAddEditModel
            {
                Title = "Brush teeth",
                Description = "Brush my teeth in the morning",
                Priority = 1,
            };
            var controller = CreateController();

            // Act
            var result = controller.UpdateTaskToDoList(id, updatedToDoList);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteTaskToDoList_ReturnsNoContentResult()
        {
            // Arrange
            var id = 1; // Assuming this ID exists
            var controller = CreateController();

            // Act
            var result = controller.DeleteTaskToDoList(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteTaskToDoList_ReturnsNotFoundResult()
        {
            // Arrange
            var id = 999; // Assuming this ID doesn't exist
            var controller = CreateController();

            // Act
            var result = controller.DeleteTaskToDoList(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void AddTaskToDoList_ReturnsInternalServerErrorWhenAddingFails()
        {
            // Arrange
            var toDoList = new ToDoListAddEditModel { };
            var controller = CreateController();
          
            // Act
            var result = controller.AddTaskToDoList(toDoList);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public void UpdateTaskToDoList_ReturnsBadRequestWhenModelInvalid()
        {
            // Arrange
            var id = 1;
            var updatedToDoList = new ToDoListAddEditModel { };
            var controller = CreateController();
            controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = controller.UpdateTaskToDoList(id, updatedToDoList);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void DeleteTaskToDoList_ReturnsInternalServerErrorWhenDeletingFails()
        {
            // Arrange
            var id = 1; // Assuming this ID exists
            var controller = CreateController();
        
            // Act
            var result = controller.DeleteTaskToDoList(id);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        private CRUDToDoListController CreateController()
        {
            var logger = new Mock<ILogger<CRUDToDoListController>>();
            var controller = new CRUDToDoListController(logger.Object);
            return controller;
        }
    }
}