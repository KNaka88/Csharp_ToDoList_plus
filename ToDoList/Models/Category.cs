using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
    public class Category
    {
        private string _name;
        private int _id;

        public Category(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category> {};
            MySqlConnection conn = DB.Connection();

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category newCategory = new Category(categoryName, categoryId);
                allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCategories;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM categories;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Category Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `categories` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int categoryId = 0;
            string categoryName = "";

            while (rdr.Read())
            {
                categoryId = rdr.GetInt32(0);
                categoryName = rdr.GetString(1);
            }

            Category foundCategory = new Category(categoryName, categoryId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundCategory;
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `categories` (`name`) VALUES (@CategoryName);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@CategoryName";
            name.Value = this._name;

            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool idEquality = (this.GetId() == newCategory.GetId());
                bool nameEquality = (this.GetName() == newCategory.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_tasks WHERE category_id = @CategoryId;", conn);

            MySqlParameter categoryIdParameter = new MySqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = this.GetId();


            cmd.Parameters.Add(categoryIdParameter);
            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddTask(Task newTask)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_tasks (category_id, task_id) VALUES (@categoryId, @taskId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@categoryId";
            category_id.Value = _id;
            cmd.Parameters.Add(category_id);

            MySqlParameter task_id = new MySqlParameter();
            task_id.ParameterName = "@TaskId";
            task_id.Value = newTask.GetId();
            cmd.Parameters.Add(task_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Task> GetTasks()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT task_id FROM categories_tasks WHERE category_id = @categoryId;";

            MySqlParameter categoryIdParameter = new MySqlParameter();
            categoryIdParameter.ParameterName = "@categoryId";
            categoryIdParameter.Value = _id;
            cmd.Parameters.Add(categoryIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<int> taskIds = new List<int> {};
            while(rdr.Read())
            {
                int taskId = rdr.GetInt32(0);
                taskIds.Add(taskId);
            }
            rdr.Dispose();

            List<Task> tasks = new List<Task>{};
            foreach (int taskId in taskIds)
            {
                var taskQuery = conn.CreateCommand() as MySqlCommand;
                taskQuery.CommandText = @"SELECT * FROM tasks WHERE id = @TaskId;";

                MySqlParameter taskIdParameter = new MySqlParameter();
                taskIdParameter.ParameterName = "@TaskId";
                taskIdParameter.Value = taskId;
                taskQuery.Parameters.Add(taskIdParameter);

                var taskQueryRdr = taskQuery.ExecuteReader() as MySqlDataReader;
                while(taskQueryRdr.Read())
                {
                    int thisTaskId = taskQueryRdr.GetInt32(0);
                    string taskDescription = taskQueryRdr.GetString(1);
                    Task foundTask = new Task(taskDescription, thisTaskId);
                    tasks.Add(foundTask);
                }
                taskQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return tasks;
        }
    }
}
