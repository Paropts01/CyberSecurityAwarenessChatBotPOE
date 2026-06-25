using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSecurityAwarenessChatBot
{
    public class TaskManager
    {
        private string connectionString;

        public TaskManager()
        {
            // Replace with your MySQL connection details
            connectionString = "Server=localhost;Database=CyberTasks;Uid=root;Pwd=yourpassword;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Title VARCHAR(255) NOT NULL,
                    Description TEXT,
                    ReminderDate DATE,
                    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
                    CreatedAt DATETIME NOT NULL
                )";

            using var cmd = new MySqlCommand(createTableQuery, conn);
            cmd.ExecuteNonQuery();
        }

        public int AddTask(string title, string description = "", string? reminderDate = null)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string insertQuery = @"
                INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted, CreatedAt)
                VALUES (@Title, @Description, @ReminderDate, FALSE, @CreatedAt);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", description ?? "");
            cmd.Parameters.AddWithValue("@ReminderDate", string.IsNullOrEmpty(reminderDate) ? (object)DBNull.Value : reminderDate);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string selectQuery = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";
            using var cmd = new MySqlCommand(selectQuery, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? null : reader.GetDateTime("ReminderDate").ToString("yyyy-MM-dd"),
                    IsCompleted = reader.GetBoolean("IsCompleted"),
                    CreatedAt = reader.GetDateTime("CreatedAt").ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            return tasks;
        }

        public bool MarkTaskAsComplete(int taskId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string updateQuery = "UPDATE Tasks SET IsCompleted = TRUE WHERE Id = @Id";
            using var cmd = new MySqlCommand(updateQuery, conn);
            cmd.Parameters.AddWithValue("@Id", taskId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteTask(int taskId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string deleteQuery = "DELETE FROM Tasks WHERE Id = @Id";
            using var cmd = new MySqlCommand(deleteQuery, conn);
            cmd.Parameters.AddWithValue("@Id", taskId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public CyberTask? GetTaskById(int taskId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string selectQuery = "SELECT * FROM Tasks WHERE Id = @Id";
            using var cmd = new MySqlCommand(selectQuery, conn);
            cmd.Parameters.AddWithValue("@Id", taskId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new CyberTask
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? null : reader.GetDateTime("ReminderDate").ToString("yyyy-MM-dd"),
                    IsCompleted = reader.GetBoolean("IsCompleted"),
                    CreatedAt = reader.GetDateTime("CreatedAt").ToString("yyyy-MM-dd HH:mm:ss")
                };
            }

            return null;
        }

        // Get tasks whose reminder date is today (for proactive notifications)
        public List<CyberTask> GetTasksWithReminderToday()
        {
            var tasks = new List<CyberTask>();
            string today = DateTime.Today.ToString("yyyy-MM-dd");

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM Tasks WHERE ReminderDate = @Today AND IsCompleted = FALSE";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Today", today);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? null : reader.GetDateTime("ReminderDate").ToString("yyyy-MM-dd"),
                    IsCompleted = reader.GetBoolean("IsCompleted"),
                    CreatedAt = reader.GetDateTime("CreatedAt").ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            return tasks;
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
