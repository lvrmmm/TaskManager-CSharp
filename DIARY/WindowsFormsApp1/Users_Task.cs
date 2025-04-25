using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace WindowsFormsApp1
{
    /// <summary>
    /// Cведения о задачах всех зарегистрированных пользователях
    /// </summary>
    public class Users_Task
    {
        private int task_id;
        private string task;
        private DateTime deadline;
        private int priority;
        private int user_id;

        public Users_Task(int task_id, string task, DateTime deadline, int priority, int user_id)
        {
            this.task_id = task_id;
            this.task = task;
            this.deadline = deadline;
            this.priority = priority;
            this.user_id = user_id;
        }

        public int Task_Id { get => task_id; set => task_id = value; }
        public string Task { get => task; set => task = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public int Priority { get => priority; set => priority = value; }
        public int User_Id { get => user_id; set => user_id = value; }


        /// <summary>
        /// Чтение данных из таблицы Users_Task в List<Users_Task> tasks
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static List<Users_Task> ReadFromDB(int user_id)
        {
            List<Users_Task> tasks = new List<Users_Task>();

            string connectionString = "Data Source=diary.db;";
            string query = "SELECT task_id, task, deadline, priority FROM Users_Task WHERE user_id = @user_id";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", user_id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int taskId = reader.GetInt32(0);
                            string task = reader.GetString(1);
                            string deadlineText = reader["deadline"].ToString();
                            DateTime deadline = DateTime.Parse(deadlineText);

                            int priority = reader.GetInt32(3);

                            Users_Task newTask = new Users_Task(taskId, task, deadline, priority, user_id);
                            tasks.Add(newTask);
                        }
                    }
                }
            }

            return tasks;
        }

    }

}
