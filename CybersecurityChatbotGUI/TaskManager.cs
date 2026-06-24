using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityChatbotGUI
{
    public class TaskManager
    {
        // IMPORTANT: Replace "your_password" with your actual MySQL password
        private string connectionString = "Server=localhost;Database=cybersecurity_chatbot;Uid=root;Pwd=Riphu*52;";

        public void AddTask(string title, string description, string reminderDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @reminder)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@reminder", string.IsNullOrEmpty(reminderDate) ? (object)DBNull.Value : reminderDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Task> GetTasks()
        {
            var tasks = new List<Task>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tasks ORDER BY created_at DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? "" : reader.GetDateTime("reminder_date").ToShortDateString(),
                            IsCompleted = reader.GetBoolean("is_completed")
                        });
                    }
                }
            }
            return tasks;
        }

        public void CompleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}