using System;
using System.IO;
using System.Windows;
using System.Drawing;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Data;
using System.Data.OleDb;

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
        }
        //проверяет textBox-ы на пустоту
        private void CheckTextBox(TextBox[] TextBoxArray)
        {
            foreach (TextBox item in TextBoxArray)
                if (item.Text == "")
                    ResultCheck(item, "Red");
                else
                    ResultCheck(item, "Red");
        }
        //проверяет значение текстового поля является ли оно числом
        private bool CheckIntTextBox(TextBox item)
        {
            int num;
            return int.TryParse(item.Text, out num) ? true : false;
        }
        //при загрузке галвного окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //при загрузке окна проверяем соединение с SQLServer
            try
            {
                connection = new SqlConnection(@"Data Source=DESKTOP-HC1JS1T\SQLEXPRESS; Initial Catalog=Flight; Integrated Security=True");
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка соединения!\n{ex.Message}", "Error!");
                this.Close();
            }
        }
        //фиксация информации занесенной в бд через интерфейс программы для восстановления данных при их потере
        private void XmlFixation(string NameFile)
        {
            /*
            try
            {
                // объект для сериализации
                InformationAirFlight Info = new InformationAirFlight(int.Parse(textBox3.Text), textBox1.Text, textBox4.Text, textBox5.Text, textBox2.Text);
                // передаем в конструктор тип класса
                XmlSerializer formatter = new XmlSerializer(typeof(InformationAirFlight));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(NameFile + (Directory.GetFiles(@".\", "*.xml", SearchOption.AllDirectories).Length + 1) + ".xml", FileMode.Append))
                {
                    formatter.Serialize(fs, Info);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }*/
        }
        private void XmlReader(string NameFile)
        {
            /*
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(InformationAirFlight));
                for (int i = 0; i < Directory.GetFiles(@".\", $@"{NameFile}*.xml", SearchOption.AllDirectories).Length; i++)
                {
                    using (FileStream fs = new FileStream("Information" + (i + 1) + ".xml", FileMode.OpenOrCreate))
                    {
                        InformationAirFlight information = (InformationAirFlight)formatter.Deserialize(fs);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }*/
        }
        //обработка нажатия кнопки "Добавить" на tabControl авиарейсы
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] TextBoxArray = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            CheckTextBox(TextBoxArray);
            Error = CheckIntTextBox(textBox3) ? Error++ : Error--;
            if (this.Error == 7)
            {
                //после проверки есть возможность добавить в БД авиарейс

                //после добавления данных в таблицу
                XmlFixation("InformationAirFlight");
                System.Windows.MessageBox.Show("Data was added in database!", "Successful!");
            }
            else
            {
                System.Windows.MessageBox.Show("Uncorrected value!\n(A red outline of the text field indicates that the entered data is incorrect)", "Error!");
            }
        }

        private void FlightView(object sender, RoutedEventArgs e)         // выводим таблицу БД в окно (снизу)
        {
            
            string cmd = "SELECT * FROM Flight";            // Из какой таблицы нужен вывод 
            SqlCommand createCommand = new SqlCommand(cmd, connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("Flight");         // В скобках указываем название таблицы
            dataAdp.Fill(dt);
            flightGrid1.ItemsSource = dt.DefaultView;       // Сам вывод 
            connection.Close();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                string cmd = "SELECT * FROM Flight";            // Из какой таблицы нужен вывод 
                SqlCommand createCommand = new SqlCommand(cmd, connection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Flight");         // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                flightGrid1.ItemsSource = dt.DefaultView;       // Сам вывод 
                connection.Close();
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Error!");
            } 
        }
    }
}
