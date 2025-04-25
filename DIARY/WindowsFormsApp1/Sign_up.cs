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

namespace WindowsFormsApp1
{
    /// <summary>
    /// Регистрация новых пользователей
    /// </summary>
    public partial class Sign_up : Form
    {
        /// <summary>
        /// Инициализация формы Sign_Up
        /// </summary>
        public Sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Устанавливает основные начальные параметры формы, такие как символ для скрытия пароля и видимость иконок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            textBox_Password.PasswordChar = '*';
            pictureBox1.Visible = false;
            textBox_Login.MaxLength = 50;
            textBox_Password.MaxLength = 50;
        }
        /// <summary>
        /// Показывает пароль при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox_Password.UseSystemPasswordChar = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }
        /// <summary>
        /// Скрывает пароль при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox_Password.UseSystemPasswordChar = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }
        /// <summary>
        /// Очищает все поля ввода при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxClear_Click(object sender, EventArgs e)
        {
            textBox_Login.Text = "";
            textBox_Password.Text = "";
        }
        /// <summary>
        /// Регистрирует нового пользователя при нажатии на кнопку "Зарегистрироваться"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (textBox_Login.Text == "" || textBox_Password.Text == "")
            {
                MessageBox.Show("Введиите данные");
                return;
            }
            Users user = new Users(textBox_Login.Text, textBox_Password.Text);
            if (user.ExistenceUser())
            {
                MessageBox.Show("Такой пользователь уже есть");
                Log_in b = new Log_in();
                b.Show();
                this.Hide();
                return;
            }
            else 
            {
                user.UpdateUser();
                MessageBox.Show("Аккаунт успешно создан!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log_in a = new Log_in();
                a.Show();
                this.Hide();
            }
            



        }
        /// <summary>
        /// Переход на форму авторизации при закрытии формы регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

    }
}
