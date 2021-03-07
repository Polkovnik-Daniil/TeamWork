using System;
using System.IO;
using System.Windows;
using System.Drawing;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Reflection;

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
                    ResultCheck(item, "Blue");
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
                connection = new SqlConnection(@"Data Source=DESKTOP-AR14CLQ\SQLEXPRESS;Initial Catalog=Kurswork;Integrated Security=True");
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка соединения!\n{ex.Message}", "Error!");
                this.Close();
            }
        }
        private object ContructorInitialisation(string NameClass, object[] ArrayElement)
        {
            try
            {
                Type type = Type.GetType($@"WpfApp1.{NameClass}");
                if (type != null)
                {
                    MethodInfo method = Type.GetType($@"WpfApp1.{NameClass}", true).GetMethod(NameClass);                                                                                                            
                    if (method != null)
                    {
                        object result = null;
                        ParameterInfo[] parameters = method.GetParameters();
                        object p = Activator.CreateInstance(type, null);
                        if (parameters.Length == 0)
                            result = method.Invoke(p, null);
                        else
                            result = method.Invoke(p, ArrayElement);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error!");
            }
            return ArrayElement;//garbage (not true)
        }
        //фиксация информации занесенной в бд через интерфейс программы для восстановления данных при их потере
        private void XmlFixation(string NameFile)
        {
            try
            {
                // Создает экземпляр объекта
                object Info = Activator.CreateInstance(Type.GetType($@"WpfApp1.{NameFile}", true));
                // объект для сериализации
                ContructorInitialisation(NameFile, ReturnInterfaceElement(NameFile));
                // передаем в конструктор тип класса
                XmlSerializer formatter = new XmlSerializer(Type.GetType(NameFile, true));
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
            }
        }
        //метод возвращает содеожимое tabconrol элементов
        private object[] ReturnInterfaceElement(string NameClass)
        {
            object[] ArrayElement = null;
            switch (NameClass)
            {
                case "InformationAirFlight":
                    ArrayElement = new object[] { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text };
                    break;
                default:
                    Error--;
                    System.Windows.MessageBox.Show("Ошибка инициализации объектов","Error!");
                    break;
            }
            return ArrayElement;
        }
        //don`t work
        private void XmlReader(string NameFile)
        {
            try
            {

                XmlSerializer formatter = new XmlSerializer(typeof(InformationAirFlight));
                for (int i = 0; i < Directory.GetFiles(@".\", $@"{NameFile}*.xml", SearchOption.AllDirectories).Length; i++)
                {
                    using (FileStream fs = new FileStream($@"{NameFile}" + (i + 1) + ".xml", FileMode.OpenOrCreate))
                    {
                        //InformationAirFlight information = (InformationAirFlight)formatter.Deserialize(fs);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        //обработка нажатия кнопки "Добавить" на tabControl авиарейсы
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] TextBoxArray = new TextBox[] { textBox1, textBox2, textBox4, textBox5 };
            CheckTextBox(TextBoxArray);
            Error = CheckIntTextBox(textBox3) ? Error + 1 : Error - 1;
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
    }
}
