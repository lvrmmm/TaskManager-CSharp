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
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Обеспечивает вход пользователя в систему
    /// </summary>
    public partial class Log_in : Form
    {
        /// <summary>
        /// Инициализация формы 
        /// </summary>
        public Log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }

        /// <summary>
        /// Устанавливает основные начальные параметры формы, такие как символ для скрытия пароля и видимость иконок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_in_Load(object sender, EventArgs e)
        {
            textBox_Password.PasswordChar = '*';
            pictureBox1.Visible = false;
            textBox_Login.MaxLength = 50;
            textBox_Password.MaxLength = 50;
        }
        /// <summary>
        /// Входит в систему и переходит на форму Plan при нажатии на кнопку "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            Users user = new Users(textBox_Login.Text, textBox_Password.Text);
            if (user.CheckingUser())
            {

                    this.Hide();
                    Plan m = new Plan(user.Id);
                    m.Show();

            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        /// <summary>
        /// Переходит на форму Sign_up при нажатии на текст-ссылку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Sign_up frm2 = new Sign_up();
            frm2.ShowDialog();
            this.Hide();
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
        /// Закрывает приложение полностью при закртии формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Log_in_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
