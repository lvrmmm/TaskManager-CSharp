using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{
    /// <summary>
    /// Cведения о всех зарегистрированных пользователях
    /// </summary>
    public class Users
    {
        private int id;
        private string login_user;
        private string password_user;

        public Users(string login_user, string password_user)
        {
            this.login_user = login_user;
            this.password_user = password_user;
        }

        public int Id { get => id; set => id = value; }
        public string Login_user { get => login_user; set => login_user = value; }
        public string Password_user { get => password_user; set => password_user = value; }

        public override string ToString()
        {
            return login_user + " " + password_user;
        }


        /// <summary>
        /// Проверяем, есть ли пользователь в базе данных
        /// </summary>
        /// <returns></returns>
        public bool CheckingUser()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = diary.db"))
            {
                connection.Open();

                string sql = $"SELECT * FROM Users WHERE login_user = '{this.login_user}' AND password_user = '{this.password_user}'";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.id = Convert.ToInt32(reader["id"]);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
    
    /// <summary>
    /// Добавляем нового пользователя
    /// </summary>
        public void UpdateUser()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=diary.db"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"INSERT INTO Users (login_user, password_user) VALUES ('{this.login_user}', '{this.password_user}')";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        /// <summary>
        /// Проверяет, зарегистрирован ли пользователь с таким именем (для регистрации)
        /// </summary>
        /// <returns></returns>

        public bool ExistenceUser()
        {

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=diary.db"))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM Users WHERE login_user = '{this.login_user}'";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public Users_Task userTask { get; set; }
    }
}