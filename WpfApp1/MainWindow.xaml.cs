using System;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Media;
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
        private SqlConnection connection;
        //проверяет textBox-ы на пустоту
        private void CheckTextBox(TextBox[] TextBoxArray)
        {
            foreach (TextBox item in TextBoxArray)
                if (item.Text == "")
                    item.BorderBrush = Brushes.Red;
                else
                    item.BorderBrush = Brushes.Blue;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] TextBoxArray = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            CheckTextBox(TextBoxArray);
        }
    }
}
