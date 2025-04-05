using SQLite;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace pomodoro_forms
{
    public class DatabaseHelper
    {
        private readonly SQLiteConnection _database;

        public DatabaseHelper()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pomodoro.db3");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<User>();
            _database.CreateTable<TaskModel>();
        }

        // ------------- USER METHODS -------------

        public int InsertUser(User user)
        {
            try
            {
                return _database.Insert(user);
            }
            catch (Exception ex)
            {
                // Handle exceptions properly
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public bool DoesUserExist(string email)
        {
            return _database.Table<User>().Any(u => u.Email == email);
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _database.Table<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public User GetUserById(int userId)
        {
            return _database.Table<User>().FirstOrDefault(u => u.Id == userId);
        }

        public int UpdateUser(User user)
    {
        return _database.Update(user);
    }

        // ------------- TASK METHODS -------------

        public int InsertTask(TaskModel task)
        {
            try
            {
                return _database.Insert(task);
            }
            catch (Exception ex)
            {
                // Handle exceptions properly
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public List<TaskModel> GetTasksByUser(int userId)
        {
            try
            {
                return _database.Table<TaskModel>().Where(t => t.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions properly
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<TaskModel>();
            }
        }

        public int UpdateTask(TaskModel task)
        {
            try
            {
                return _database.Update(task);
            }
            catch (Exception ex)
            {
                // Handle exceptions properly
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public int DeleteTask(int taskId)
        {
            try
            {
                return _database.Delete<TaskModel>(taskId);
            }
            catch (Exception ex)
            {
                // Handle exceptions properly
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
