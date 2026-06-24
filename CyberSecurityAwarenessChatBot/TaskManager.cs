using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace CyberSecurityAwarenessChatBot
{
    public class TaskManager
    {
        private string connectionString;

        public TaskManager()
        {
            // Create the database file if it doesn't exist
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CyberTasks.db");
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    ReminderDate TEXT,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL
                )";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public int AddTask(string title, string description = "", string reminderDate = null)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string insertQuery = @"
                INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted, CreatedAt)
                VALUES (@Title, @Description, @ReminderDate, 0, @CreatedAt);
                SELECT last_insert_rowid();";

            using var command = new SQLiteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Description", description ?? "");
            command.Parameters.AddWithValue("@ReminderDate", reminderDate);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string selectQuery = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";
            using var command = new SQLiteCommand(selectQuery, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    ReminderDate = reader["ReminderDate"] != DBNull.Value ? reader["ReminderDate"].ToString() : null,
                    IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                    CreatedAt = reader["CreatedAt"].ToString()
                });
            }

            return tasks;
        }

        public bool MarkTaskAsComplete(int taskId)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string updateQuery = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";
            using var command = new SQLiteCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@Id", taskId);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteTask(int taskId)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string deleteQuery = "DELETE FROM Tasks WHERE Id = @Id";
            using var command = new SQLiteCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@Id", taskId);

            return command.ExecuteNonQuery() > 0;
        }

        public CyberTask? GetTaskById(int taskId)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string selectQuery = "SELECT * FROM Tasks WHERE Id = @Id";
            using var command = new SQLiteCommand(selectQuery, connection);
            command.Parameters.AddWithValue("@Id", taskId);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new CyberTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    ReminderDate = reader["ReminderDate"] != DBNull.Value ? reader["ReminderDate"].ToString() : null,
                    IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                    CreatedAt = reader["CreatedAt"].ToString()
                };
            }

            return null;
        }
    }

    public class CyberTask
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? CreatedAt { get; set; }

        public override string ToString()
        {
            string status = IsCompleted ? "✓ Completed" : "○ Pending";
            string reminder = !string.IsNullOrEmpty(ReminderDate) ? $" (Reminder: {ReminderDate})" : "";
            return $"{status} - {Title}{reminder}";
        }
    }
}
