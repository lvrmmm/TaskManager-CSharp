using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Создание новой задачи, добавление её в базу данных
    /// </summary>
    public partial class Newtask : Form
    {
        private int id;
        
        /// <summary>
        /// Инициализация формы Newtask, определение её положение на экране
        /// </summary>
        /// <param name="id"></param>
        public Newtask(int id)
        {
            this.id = id;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Добавление в базу данных новой задачи при заполнение полей ввода и нажатии на кнопку "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            

            var task = textBox1_task.Text;
            var priority = textBox2_priority.Text;
            var data = textBox3_time.Text;
            DateTime time;
            SQLiteConnection connection = new SQLiteConnection("Data Source = diary.db");
            connection.Open();
            if (DateTime.TryParse(data, out time))
            {
                if (0<=Convert.ToInt32(priority) && Convert.ToInt32(priority) <= 10)
                {
                    var newtaskquery = $"insert into Users_Task(task,deadline, priority, user_id) values('{task}','{time}','{priority}', '{id}')";
                    var command = connection.CreateCommand();
                    command.CommandText = newtaskquery;
                    command.ExecuteNonQuery();
                    MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Неверное значение приоритета! \n Укажите значение от 0 до 10!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                

                
            }
            else
            {

                MessageBox.Show("Неверно введено время исполнения!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
           connection.Close();
        }
        /// <summary>
        /// Очистка полей ввода при нажатии на кнопку 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox1_task.Text = "";
            textBox2_priority.Text = "";
            textBox3_time.Text = "";
        }

    }
}
