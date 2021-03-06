using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;


namespace ToDoList.Tests
{
    [TestClass]
    public class TaskTests : IDisposable
    {
        public void Dispose()
        {
            Task.DeleteAll();
            Category.DeleteAll();
        }
        public TaskTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test";
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = Task.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_TaskList()
        {
            //Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            //Act
            List<Task> result = Task.GetAll();
            List<Task> testList = new List<Task>{testTask};

            CollectionAssert.AreEqual(testList, result);
        }

        // [TestMethod]
        // public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Task()
        // {
        //     // Arrange, Act
        //     Task firstTask = new Task("Mow the lawn");
        //     Task secondTask = new Task("Mow the lawn");
        //
        //     // Assert
        //     Assert.AreEqual(firstTask, secondTask);
        // }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            //Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            // Act
            Task savedTask = Task.GetAll()[0];

            int result = savedTask.GetId();
            int testId = testTask.GetId();

            // Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsTaskInDatabase_Task()
        {
            // Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            // Act
            Task foundTask = Task.Find(testTask.GetId());

            // Assert
            Assert.AreEqual(testTask, foundTask);

        }

        // [TestMethod]
        // public void Update_UpdatesTaskInDatabase_String()
        // {
        //     // Arrange
        //     string firstDescription = "Walk the Dog";
        //     Task testTask = new Task(firstDescription);
        //     testTask.Save();
        //     string secondDescription = "Mow the lawn";
        //
        //     // Act
        //     testTask.UpdateDescription(secondDescription);
        //
        //     string result = Task.Find(testTask.GetId()).GetDescription();
        //
        //     // Assert
        //     Assert.AreEqual(secondDescription, result);
        // }

        // [TestMethod]
        // public void Delete_DeleteTaskFromDatabase()
        // {
        //     // Arrange
        //     string firstDescription = "Walk the Dog";
        //     Task testTask = new Task(firstDescription);
        //     testTask.Save();
        //     int taskId = testTask.GetId();
        //
        //     // Act
        //     testTask.Delete();
        //     int result = Task.Find(taskId).GetId();
        //
        //     // Assert
        //     Assert.AreEqual(0, result);
        // }

        [TestMethod]
        public void AddCategory_AddsCategoryToTask_CategoryList()
        {
            // Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            Category testCategory = new Category("Home stuff");
            testCategory.Save();

            //Act
            testTask.AddCategory(testCategory);

            List<Category> result = testTask.GetCategories();
            List<Category> testList = new List<Category>{testCategory};

            // Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetCategories_ReturnsAllTaskCategories_CategoryList()
        {
            // Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            Category testCategory1 = new Category("Home stuff");
            testCategory1.Save();

            Category testCategory2 = new Category("Work stuff");
            testCategory2.Save();

            // Act
            testTask.AddCategory(testCategory1);
            List<Category> result = testTask.GetCategories();
            List<Category> testList = new List<Category> {testCategory1};

            // Assert
            CollectionAssert.AreEqual(testList, result);
        }
    }
}


// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System.Collections.Generic;
// using System;
// using ToDoList.Models;
//
// namespace ToDoList.Tests
// {
//     [TestClass]
//     public class TaskTest : IDisposable
//     {
//         public void Dispose()
//         {
//             Task.ClearAll();
//         }
//
//         [TestMethod]
//         public void GetDescription_ReturnsDescription_String()
//         {
//             //Arrange
//             string description = "Walk the dog.";
//             Task newTask = new Task(description);
//
//             //Act
//             string result = newTask.GetDescription();
//
//             //Assert
//             Assert.AreEqual(description, result);
//         }
//
//         [TestMethod]
//         public void GetAll_ReturnsTasks_TaskList()
//         {
//             //Arrange
//             string description01 = "Walk the dog";
//             string description02 = "Wash the dishes";
//             Task newTask1 = new Task(description01);
//             Task newTask2 = new Task(description02);
//             List<Task> newList = new List<Task> { newTask1, newTask2 };
//
//             //Act
//             List<Task> result = Task.GetAll();
//
//             foreach (Task thisTask in result)
//             {
//                 Console.WriteLine("Output: " + thisTask.GetDescription());
//             }
//
//             //Assert
//             CollectionAssert.AreEqual(newList, result);
//         }
//     }
// }
