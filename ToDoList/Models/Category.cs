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

        public List<Task> GetTasks()
        {
            List<Task> allCategoryTasks = new List<Task> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id;";

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@category_id";
            categoryId.Value = this._id;
            cmd.Parameters.Add(categoryId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int taskId = rdr.GetInt32(0);
                string taskDescription = rdr.GetString(1);
                Task newTask = new Task(taskDescription, taskId);
                allCategoryTasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCategoryTasks;
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
    }
}
