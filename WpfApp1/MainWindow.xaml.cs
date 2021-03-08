using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Data;

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
        //читает информацию с Xml BackUp---------------------------------------------------------------
        private void XmlReader(string NameFile)
        {
            try
            {
                Type tp=Type.GetType(NameFile);
                XmlSerializer formatter = new XmlSerializer(Type.GetType(NameFile, true));
                for (int i = 0; i < Directory.GetFiles(@".\", $@"{NameFile}*.xml", SearchOption.AllDirectories).Length; i++)
                {
                    using (FileStream fs = new FileStream($@"{NameFile}" + (i + 1) + ".xml", FileMode.OpenOrCreate))
                    {

                        var information = (InformationAirFlight)formatter.Deserialize(fs);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        //фиксация информации занесенной в бд через интерфейс программы для восстановления данных при их потере
        private void XmlFixation(string NameFile)
        {
            try
            {
                var Info = Activator.CreateInstance(Type.GetType($"WpfApp1.{NameFile}"), ReturnInterfaceElement(NameFile));
                XmlSerializer formatter = new XmlSerializer(Type.GetType($"WpfApp1.{NameFile}"));
                using (FileStream fs = new FileStream(NameFile + (Directory.GetFiles(@".\", $"{NameFile}*.xml", SearchOption.AllDirectories).Length + 1) + ".xml", FileMode.Append))
                {
                    formatter.Serialize(fs, Info);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        //проверяет значение текстового поля является ли оно числом
        private bool CheckIntTextBox(TextBox item)
        {
            int num;
            return int.TryParse(item.Text, out num) ? true : false;
        }
        //отображение таблицы
        private void DisplayTable(string TableName)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                SqlCommand createCommand = new SqlCommand($"SELECT * FROM {TableName}", connection);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable($"{TableName}");                                           // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                flightGrid1.ItemsSource = dt.DefaultView;                                               // Сам вывод 
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
        //проверяет textBox-ы на пустоту
        private void CheckTextBox(TextBox[] TextBoxArray)
        {
            foreach (TextBox item in TextBoxArray)
                if (item.Text == "")
                    ResultCheck(item, "Red");
                else
                    ResultCheck(item, "Blue");
        }
        //алгоритм проверки корректности введенных данных
        private int ResultCheck(TextBox item, string color)
        {
            switch (color)
            {
                case "Blue":
                    item.BorderBrush = System.Windows.Media.Brushes.Blue;
                    Error++;
                    break;
                case "Red":
                    item.BorderBrush = System.Windows.Media.Brushes.Red;
                    Error--;
                    break;
                default:
                    System.Windows.MessageBox.Show("Error conflict situation!", "Error!");
                    Error--;
                    break;
            }
            return Error;
        }
        //метод возвращает содеожимое tabconrol элементов
        private object[] ReturnInterfaceElement(string NameClass)
        {
            switch (NameClass)
            {
                case "InformationAirFlight":
                    return new object[] { int.Parse(textBox3.Text), textBox1.Text, textBox4.Text, textBox2.Text, textBox5.Text };
                default:
                    Error--;
                    MessageBox.Show("Ошибка инициализации объектов","Error!");
                    break;
            }
            return null;
        }
        //обработка нажатия кнопки "Обновить" на tabControl авиарейсы
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayTable("Table_1");
        }
        //обработка нажатия кнопки "Добавить" на tabControl авиарейсы
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] TextBoxArray = new TextBox[] { textBox1, textBox2, textBox4, textBox5 };
            CheckTextBox(TextBoxArray);
            Error = CheckIntTextBox(textBox3) ? ResultCheck(textBox3, "Blue") : ResultCheck(textBox3, "Red");
            XmlFixation("InformationAirFlight");
            if (this.Error == 5)
            {
                //после проверки есть возможность добавить в БД авиарейс

                //после добавления данных в таблицу
                System.Windows.MessageBox.Show("Data was added in database!", "Successful!");
            }
            else
            {
                System.Windows.MessageBox.Show("Uncorrected value!\n(A red outline of the text field indicates that the entered data is incorrect)", "Error!");
            }
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
                MessageBox.Show($"Ошибка соединения!\n{ex.Message}", "Error!");
                this.Close();
            }
        }
        // выводим таблицу БД в окно (снизу)
        private void FlightViewTable_Loaded(object sender, RoutedEventArgs e)         
        {
            DisplayTable("Table_1");
        }
    }
}