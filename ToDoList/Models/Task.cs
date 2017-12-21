using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList;

namespace ToDoList.Models
{
    public class Task
    {
        private string _description;
        private int _id;
        private int _categoryId;

        public Task(string description, int categoryId, int id = 0)
        {
            _id = id;
            _categoryId = categoryId;
            _description = description;
        }

        public override bool Equals(System.Object otherTask)
        {
            if (!(otherTask is Task))
            {
                return false;
            }
            else
            {
                Task newTask = (Task) otherTask;
                bool idEquality = (this.GetId() == newTask.GetId());
                bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();
                bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
                return (idEquality && descriptionEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetDescription().GetHashCode();
        }

        public string GetDescription()
        {
            return _description;
        }

        public int GetCategoryId()
        {
            return _categoryId;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        public static List<Task> GetAll()
        {
            List<Task> allTasks = new List<Task> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int taskId = rdr.GetInt32(0);
                string taskDescription = rdr.GetString(1);
                int taskCategoryId = rdr.GetInt32(2);
                Task newTask = new Task(taskDescription, taskCategoryId, taskId);
                allTasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allTasks;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Task Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks WHERE id = (@searchId);";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@searchId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int taskId = 0;
            int taskCategoryId = 0;
            string taskDescription = "";

            while (rdr.Read())
            {
                taskId = rdr.GetInt32(0);
                taskDescription = rdr.GetString(1);
                taskCategoryId = rdr.GetInt32(2);
            }

            Task foundTask = new Task(taskDescription, taskId, taskId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundTask;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO tasks (description, category_id) VALUES (@description, @category_id);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@category_id";
            categoryId.Value = this._categoryId;
            cmd.Parameters.Add(categoryId);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
