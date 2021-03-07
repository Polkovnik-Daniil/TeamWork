using System;
using System.Windows;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private int Error = 0;
        private SqlConnection connection;
        //алгоритм проверки корректности введенных данных
        private void ResultCheck(TextBox item, string color)
        {
            switch (color)
            {
                case "Blue":
                    item.BorderBrush = System.Windows.Media.Brushes.Blue;
                    break;
                case "Red":
                    item.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
            }
        }
        //проверяет textBox-ы на пустоту
        private void CheckTextBox(TextBox[] TextBoxArray)
        {
            foreach (TextBox item in TextBoxArray)
                if (item.Text == "")
                    item.BorderBrush = System.Windows.Media.Brushes.Red;
                else
                    item.BorderBrush = System.Windows.Media.Brushes.Blue;
        }
        //при загрузке галвного окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //при загрузке окна проверяем соединение с SQLServer
            try
            {
                connection = new SqlConnection(@"Data Source=DESKTOP-AR14CLQ\SQLEXPRESS;Initial Catalog=Kurswork;Integrated Security=True");
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка соединения!\n{ex.Message}", "Error!");
                this.Close();
            }
        }
        //обработка нажатия кнопки "Добавить"
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] TextBoxArray = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            CheckTextBox(TextBoxArray);

            if (this.Error == 7)
            {
                //после проверки есть возможность добавить в БД авиарейс
            }
        }
    }
}
