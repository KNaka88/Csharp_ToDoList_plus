using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Tests
{
    [TestClass]
    public class CategoryTests : IDisposable
    {
        public CategoryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        public void Dispose()
        {
            Task.DeleteAll();
            Category.DeleteAll();
        }

        [TestMethod]
        public void GetAll_CategoriesEmptyAtFirst_0()
        {
            // Arrange, Act
            int result = Category.GetAll().Count;

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueForSameName_Category()
        {
            // Arrange, Act
            Category firstCategory = new Category("Household chores");
            Category secondCategory = new Category("Household chores");

            // Assert
            Assert.AreEqual(firstCategory, secondCategory);
        }

        [TestMethod]
        public void Save_SavesCategoryToDatabase_CategoryList()
        {
            // Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            // Act
            List<Category> result = Category.GetAll();
            List<Category> testList = new List<Category>{testCategory};

            // Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_DatabaseAssignsIdToCategory_Id()
        {
            // Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            // Act
            Category savedCategory = Category.GetAll()[0];

            int result = savedCategory.GetId();
            int testId = testCategory.GetId();

            // Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsCategoryInDatabase_Category()
        {
            // Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            // Act
            Category foundCategory = Category.Find(testCategory.GetId());

            // Assert
            Assert.AreEqual(testCategory, foundCategory);
        }

        // [TestMethod]
        // public void GetTasks_RetrievesAllTasksWithCategory_TaskList()
        // {
        //     Category testCategory = new Category("Household chores");
        //     testCategory.Save();
        //
        //     Task firstTask = new Task("Mow the lawn");
        //     firstTask.Save();
        //
        //     Task secondTask = new Task("Do the dishes");
        //     secondTask.Save();
        //
        //     List<Task> testTaskList = new List<Task> {firstTask, secondTask};
        //     List<Task> resultTaskList = testCategory.GetTasks();
        //
        //     CollectionAssert.AreEqual(testTaskList, resultTaskList);
        // }

        [TestMethod]
        public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
        {
            // Arrange
            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            string testName = "Home stuff";
            Category testCategory = new Category(testName);
            testCategory.Save();

            // Act
            testCategory.AddTask(testTask);
            testCategory.Delete();

            List<Category> resultTaskCategories = testTask.GetCategories();
            List<Category> testTaskCategories = new List<Category>{};

            // Assert
            CollectionAssert.AreEqual(testTaskCategories, resultTaskCategories);
        }

        [TestMethod]
        public void Test_AddTask_AddTaskToCategory()
        {
            // Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Task testTask = new Task("Mow the lawn");
            testTask.Save();

            Task testTask2 = new Task("Water the garden");
            testTask2.Save();

            // Act
            testCategory.AddTask(testTask);
            testCategory.AddTask(testTask2);

            List<Task> result = testCategory.GetTasks();
            List<Task> testList = new List<Task>{testTask, testTask2};

            // Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetTasks_ReturnsAllCategoryTasks_TaskList()
        {
            // Arrange
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Task testTask1 = new Task("Mow the lawn");
            testTask1.Save();

            Task testTask2 = new Task("Buy plane ticket");
            testTask2.Save();

            // Act
            testCategory.AddTask(testTask1);
            List<Task> savedTasks = testCategory.GetTasks();
            List<Task> testList = new List<Task> {testTask1};

            // Assert
            CollectionAssert.AreEqual(testList, savedTasks);
        }
    }
}
