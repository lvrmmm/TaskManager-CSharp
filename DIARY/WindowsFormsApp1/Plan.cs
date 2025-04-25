using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Timers;
using System.Runtime.InteropServices;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Выполнение основных функций "Ежедневника" таких, как отображение списка задач, поиск задач и др.
    /// </summary>
    public partial class Plan : Form

    {
        private int id;
        int selectedRow;
        /// <summary>
        /// Инициализация формы Plan
        /// </summary>
        /// <param name="id"></param>
        public Plan(int id)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.id = id;

        }

        /// <summary>
        /// Создание колонок DataGridView
        /// </summary>
        private void CreateColumns()
        {

            dataGridView1.Columns.Add("task_id", "Номер задачи");
            dataGridView1.Columns.Add("task", "Задача");
            dataGridView1.Columns.Add("deadline", "Срок выполнения");
            dataGridView1.Columns.Add("priority", "Приоритет");
            dataGridView1.Columns.Add("userid", "Пользователь");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }



        /// <summary>
        /// Обновление DataGridView (очистка полей и новая запись из List)
        /// </summary>
        /// <param name="dgw"></param>
        private void RefreshDataGridView(DataGridView dgw)
        {

            dgw.Rows.Clear();

            List <Users_Task> Tasks = Users_Task.ReadFromDB(id);
            foreach (Users_Task usertask in Tasks)
            {
                dgw.Rows.Add(usertask.Task_Id, usertask.Task, usertask.Deadline, usertask.Priority, usertask.User_Id);
            }

        }
        /// <summary>
        /// Поиск в базе данных по введёному в поле тексту
        /// </summary>
        /// <param name="dgw"></param>
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            var connection = new SQLiteConnection("Data Source = diary.db");
            string searchstring = $"select * from Users_Task where (task like '%" + textBox_Search.Text + "%' OR deadline like '%" + textBox_Search.Text + "%' OR priority like '%" + textBox_Search.Text + $"%' ) AND user_id = '{id}'";
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = searchstring;

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int taskId = reader.GetInt32(0);
                string task = reader.GetString(1);
                string deadlineText = reader["deadline"].ToString();
                DateTime deadline = DateTime.Parse(deadlineText);

                int priority = reader.GetInt32(3);

                dgw.Rows.Add(taskId, task, deadline, priority, id);
            }
            connection.Close();

        }
        /// <summary>
        /// Поиск задачи при введение текста в TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        /// <summary>
        /// Очистка всех полей для ввода
        /// </summary>
        private void ClearTextBox()
        {
            textBox1_Task_id.Text = "";
            textBox1_task.Text = "";
            textBox3_time.Text = "";
            textBox2_priority.Text = "";
            textBox_Search.Text = "";

        }
        /// <summary>
        /// Очистка всех полей при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ClearTextBox();
        }

        /// <summary>
        /// Удаление выбранной строки из DataGridView и базы данных
        /// </summary>
        /// <param name="user_id"></param>
        private void DeleteSelectedRow(int user_id)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления! \n" +
                                "Выберите для этого крайнюю левую ячейку в строке нужной задачи!", "Укажите строку!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            int selectedTaskId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["task_id"].Value);

            string connectionString = "Data Source=diary.db;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string deletingstring = $"DELETE FROM Users_Task WHERE user_id= '{id}' AND task_id='{selectedTaskId}'";
                    command.CommandText = deletingstring;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

            MessageBox.Show("Задача успешно удалена!");
        }

        /// <summary>
        /// Удаляет задачу из базы данных и DataGridView при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow(id);
            ClearTextBox();
            RefreshDataGridView(dataGridView1);
        }

        /// <summary>
        /// Обновляет данные в DataGridView и в базе данных
        /// </summary>

        private void ChangeData()
        {
           
            int task_id;
            if (!int.TryParse(textBox1_Task_id.Text, out task_id))
            {
                MessageBox.Show("Введите корректное значение для Task ID", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string task = textBox1_task.Text;

            DateTime deadline;
            if (!DateTime.TryParse(textBox3_time.Text, out deadline))
            {
                MessageBox.Show("Введите корректное значение для срока исполнения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int priority;
            if (!int.TryParse(textBox2_priority.Text, out priority))
            {
                
                MessageBox.Show("Введите корректное значение для Priority", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!(0 <= priority && priority <= 10))
                {
                    MessageBox.Show("Введите значение приоритета в пределах от 0 до 10!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                row.Cells["task_id"].Value = task_id;
                row.Cells["task"].Value = task;
                row.Cells["deadline"].Value = deadline;
                row.Cells["priority"].Value = priority;
            }


            string connectionString = "Data Source=diary.db;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string updatingquery = $"UPDATE Users_Task SET task = '{task}', deadline = '{deadline}', priority = '{priority}' WHERE task_id = '{task_id}' and user_id = '{id}'";
                    command.CommandText = updatingquery;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Данные успешно обновлены", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshDataGridView(dataGridView1);

        }
        /// <summary>
        /// Изменяет данные задачи в базе данных и DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_change_Click(object sender, EventArgs e)
        {
            ChangeData();  
            ClearTextBox();
        }

        /// <summary>
        /// Получение загруженности недели
        /// </summary>
        /// <param name="tasks"></param>
        private void GetCountTasks(List<Users_Task> tasks)
        {
            Dictionary<string, int> tasksPerDay = new Dictionary<string, int>();

            DateTime now = DateTime.Now.Date;
            DateTime endDate = now.AddDays(7).Date;


            for (DateTime date = now; date < endDate; date = date.AddDays(1))
            {
                int count = tasks.Count(t => t.Deadline.Date == date.Date);
                tasksPerDay.Add(date.ToShortDateString(), count);

            }
            foreach (KeyValuePair<string, int> pair in tasksPerDay)
            {
                chart1.Series[0].Points.AddXY(pair.Key, pair.Value);
            }

            string maxTasksDay = string.Join(", ", tasksPerDay.Where(x => x.Value == tasksPerDay.Max(y => y.Value)).Select(z => z.Key));
            textBox_Day.Text = maxTasksDay;

            int maxTasks = tasksPerDay.Max(x => x.Value);
            textBox_CountDay.Text = Convert.ToString(maxTasks);

            int misstask = tasks.Count(t => t.Deadline < DateTime.Now);
            textBox_misstask.Text = Convert.ToString(misstask);

        }
        /// <summary>
        /// Получение загруженности недели при нажатии на кнопку "Узнать загруженность"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Count_Tasks_Click(object sender, EventArgs e)
        {

            List<Users_Task> Tasks = Users_Task.ReadFromDB(id);
            chart1.Series[0].Points.Clear();
            GetCountTasks(Tasks);

        }
        /// <summary>
        /// Инициализация DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_Form_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGridView(dataGridView1);
            Search(dataGridView1);
            
        }


        /// <summary>
        /// Вывод данных о задаче в поля, расположенный снизу формы Plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox1_Task_id.Text = row.Cells[0].Value.ToString();
                textBox1_task.Text = row.Cells[1].Value.ToString();
                textBox3_time.Text = row.Cells[2].Value.ToString();
                textBox2_priority.Text = row.Cells[3].Value.ToString();



            }
        }
        /// <summary>
        /// Обновление таблицы DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            RefreshDataGridView(dataGridView1);
            ClearTextBox();
        }

        /// <summary>
        /// Определение новой задачи, переход на форму Newtask
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_Click(object sender, EventArgs e)
        {
            Newtask newtask = new Newtask(id);
            newtask.Show();
        }
        
        /// <summary>
        /// Полное закрытие приложения "Ежедневник"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Plan_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }

}

