using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExampleWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int curFontSize = 14;
        Dictionary<string, string> fontSizesDict = new Dictionary<string, string>() ;

        public MainWindow()
        {
            InitializeComponent();

            //Устанавливаем шрифт и размер по умолчанию
            font1menuItem.IsChecked = true;
            textBox.FontFamily = new FontFamily("Times New Roman");
            textBox.FontSize = 10;

            //set default fontSize in fontSizeTextBox
            fontSizeTextBox.Text = curFontSize.ToString();

            //Заполняем словарь значениями для размера шрифта
            setUpDict();

            //добавляем в комбобокс варианты выбора размера шрифта
            setUpCombpBoxFontSize();

            tabControlItemEditor.IsEnabled = false;
        }

        //ф для добавления в комбобокс размеров шрифта
        private void setUpCombpBoxFontSize()
        {
            foreach(KeyValuePair<string, string> elem in fontSizesDict)
            {
                fontSizeComboBox.Items.Add(elem.Value);
            }
            fontSizeComboBox.SelectedIndex = 2;
        }

        //Ф для заполнения массива размеров шрифтов
        private void setUpDict()
        {
            fontSizesDict.Add("0", "10");
            fontSizesDict.Add("1", "12");
            fontSizesDict.Add("2", "14");
            fontSizesDict.Add("3", "16");
            fontSizesDict.Add("4", "18");
            fontSizesDict.Add("5", "20");
            fontSizesDict.Add("6", "22");
            fontSizesDict.Add("7", "24");
            fontSizesDict.Add("8", "26");
            fontSizesDict.Add("9", "28");
            fontSizesDict.Add("10", "30");
            
            
            //Последний элемент всегда ставим прочерк
            fontSizesDict.Add("11", "-");
        }

        private void newFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.Text != "")
            {
                saveFile();
            }
            textBox.Text = "";
        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? res = ofd.ShowDialog();

            if (res == false) return;

            //If we can open file? then it won't be null
            Stream stream;
            if((stream = ofd.OpenFile()) != null)
            {
                string file_name = ofd.FileName;
                string text_from_file = File.ReadAllText(file_name);
                textBox.Text = text_from_file;
            }
        }

        private void saveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }
         
        private void saveFile()
        {
            //Встроенный класс, который отображает диалоговое окно для сохранения файла
            SaveFileDialog sfd = new SaveFileDialog();
            bool? res = sfd.ShowDialog();

            if (res == false) return;

            //Далее записываем в файл, ктоторый мы создали в всплывающем окне, нашу информацию
            using (Stream s = File.Open(sfd.FileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(textBox.Text.ToString());
                }
            }
        }

        private void font1menuItem_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Times New Roman");
            font2menuItem.IsChecked = false;
        }

        private void font2menuItem_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Verdana");
            font1menuItem.IsChecked = false;
        }

        //Метод при закрытии меню
        private void Size_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            string str = fontSizeTextBox.Text.ToString();
            //If empty body, take current font size
            if(str == "" || str == "0" || str == "00")
            {
                fontSizeTextBox.Text = curFontSize.ToString();
                return;
            }
            //else get size from textBox
            curFontSize = Convert.ToInt32(fontSizeTextBox.Text.ToString());
            textBox.FontSize = curFontSize;

            //Если есть такое значение в комбобоксе, которое мы ввели, то устанавливаем его
            trySetUpComboBoxFontSize(curFontSize);
        }

        //Пытаемся установить в сомбобокс значение шрифта, которое мы ввели вручную
        private void trySetUpComboBoxFontSize(int curFontSize)
        {
            foreach(KeyValuePair<string, string> elem in fontSizesDict)
            {
                if(elem.Value == curFontSize.ToString())
                {
                    fontSizeComboBox.SelectedIndex = Convert.ToInt32(elem.Key);
                    return;
                }
            }
            //Иначе ставим прочерк - последний элемент всегда добавляем прочерк
            fontSizeComboBox.SelectedIndex = fontSizesDict.Count - 1;
        }


        //Вызывается при вводе какого-либо символа в ткстовое поле. Если это не число, то нельзя его ввести
        private void fontSizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }

        //If was pressed backspace, then dont input it 
        private void fontSizeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        //Обработчик событий выбора из выподащго меню для фона
        private void fontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Принимаем значение из ComboBox
            string fontSizeStr = fontSizeComboBox.SelectedItem.ToString();
            
            try
            {
                fontSizeStr = fontSizeStr.Substring(fontSizeStr.Length - 2);
                curFontSize = Convert.ToInt32(fontSizeStr);
                fontSizeTextBox.Text = curFontSize.ToString();
                textBox.FontSize = curFontSize;
            } catch
            {
                return;
            }
        }

        //При нажатии на энтер переход на следующую строку
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                textBox.Text += '\n';
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void btnRegistrationUser_Click(object sender, RoutedEventArgs e)
        {
            string connStr = ConfigurationManager.AppSettings["connectionString"];

            SqlConnection sqlConn = new SqlConnection(connStr);

            string curLogin = textBoxLogin.Text;
            string curPassword = textBoxPassword.Password;

            if(curLogin == "" || curPassword == "")
            {
                MessageBox.Show("Empty Fields!");
                return;
            }

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                string query = "SELECT COUNT(1) FROM Users WHERE login=@login";
                SqlCommand sqlComm = new SqlCommand(query, sqlConn);
                sqlComm.CommandType = System.Data.CommandType.Text;
                sqlComm.Parameters.AddWithValue("@login", curLogin);

                int countUser = Convert.ToInt32(sqlComm.ExecuteScalar());
                if(countUser == 1)
                {
                    MessageBox.Show("This login already exists!");
                    return;
                }

                if(countUser == 0)
                {
                    string queryAddUser = "INSERT INTO Users(login, password) VALUES(@login, @pass);";
                    SqlCommand sqlCommAdd = new SqlCommand(queryAddUser, sqlConn);
                    sqlCommAdd.CommandType = System.Data.CommandType.Text;
                    sqlCommAdd.Parameters.AddWithValue("@login", curLogin);
                    sqlCommAdd.Parameters.AddWithValue("@pass", curPassword);

                    sqlCommAdd.ExecuteNonQuery();
                    MessageBox.Show("You have been saved");
                    
                } 
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        //Удаляем пользователя по нажатию на кнопку, если совпали логин и пароль
        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            string connStr = ConfigurationManager.AppSettings["connectionString"];
            SqlConnection sqlConn = new SqlConnection(connStr);

            string curLogin = textBoxLogin.Text.ToString();
            string curPassword = textBoxPassword.Password.ToString();

            if (curLogin == "" || curPassword == "")
            {
                MessageBox.Show("Empty Fields!");
                return;
            }

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                //firstly we should count number of this users. If no such users with that password and login then do nothing
                string queryCheck = "SELECT COUNT(1) FROM Users WHERE login=@login AND password=@password;";
                SqlCommand sqlCommandCheck = new SqlCommand(queryCheck, sqlConn);
                sqlCommandCheck.CommandType = System.Data.CommandType.Text;
                sqlCommandCheck.Parameters.AddWithValue("@login", curLogin);
                sqlCommandCheck.Parameters.AddWithValue("@password", curPassword);

                int numberOfUsers = Convert.ToInt32(sqlCommandCheck.ExecuteScalar());
                if(numberOfUsers == 0)
                {
                    MessageBox.Show("Can't delete! No users with such password and login");
                    return;
                }

                string queryStr = "DELETE FROM Users WHERE login=@login AND password=@password;";
                SqlCommand sqlCommand = new SqlCommand(queryStr, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@login", curLogin);
                sqlCommand.Parameters.AddWithValue("@password", curPassword);

                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("You have been Deleted!");
                
                textBoxLogin.Text = "";
                textBoxPassword.Password = "";
                tabControlItemEditor.IsEnabled = false;

            } catch(Exception ex)
            {
                MessageBox.Show($"(kek) {ex.Message}");
            }
            finally
            {
                sqlConn.Close();
            }
            
        }

        private void btnEnterUser_Click(object sender, RoutedEventArgs e)
        {
            string connStr = ConfigurationManager.AppSettings["connectionString"];
            SqlConnection sqlConn = new SqlConnection(connStr);

            string curLogin = textBoxLogin.Text;
            string curPassword = textBoxPassword.Password;

            if (curLogin == "" || curPassword == "")
            {
                MessageBox.Show("Empty Fields!");
                return;
            }

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                string queryStr = "SELECT COUNT(1) FROM Users WHERE login=@login";
                SqlCommand sqlCommand = new SqlCommand(queryStr, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@login", curLogin);

                //Узнаем сколько человек с таким логином в базе данных
                int countUser = Convert.ToInt32(sqlCommand.ExecuteScalar());

                if(countUser == 0)
                {
                    MessageBox.Show("Firstly you should reg. No users with such login!");
                    return;
                }

                //Если у нас не равно нулю количество людей с таким логином в базе данных, 
                //то проверяем есть ли с таки логином и паролем
                string queryStr2 = "SELECT COUNT(1) FROM Users WHERE login=@login AND password=@password;";
                SqlCommand sqlCommand2 = new SqlCommand(queryStr2, sqlConn);
                sqlCommand2.CommandType = System.Data.CommandType.Text;
                sqlCommand2.Parameters.AddWithValue("@login", curLogin);
                sqlCommand2.Parameters.AddWithValue("@password", curPassword);

                int countUnicUser =Convert.ToInt32(sqlCommand2.ExecuteScalar());

                if(countUnicUser == 0) {
                    MessageBox.Show("Wrong password!");
                    return;
                }

                AuthPage authPage = new AuthPage();
                authPage.Show();
                tabControlItemEditor.IsEnabled = true;
                tabControlItemEditor.IsSelected = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}
