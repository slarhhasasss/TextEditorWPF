using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            //Устанавливаем шрифт и размер по умолчанию
            font1menuItem.IsChecked = true;
            textBox.FontFamily = new FontFamily("Times New Roman");
            textBox.FontSize = 10;

            //set default fontSize in fontSizeTextBox
            fontSizeTextBox.Text = curFontSize.ToString();
        }

        private void newFileMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
        }

        private void saveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Встроенный класс, который отображает диалоговое окно для сохранения файла
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
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

        private void Size_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            string str = fontSizeTextBox.Text.ToString();
            //If empty body, take current font size
            if(str == "")
            {
                fontSizeTextBox.Text = curFontSize.ToString();
                return;
            }
            //else get size from textBox
            curFontSize = Convert.ToInt32(fontSizeTextBox.Text.ToString());
            textBox.FontSize = curFontSize;
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
    }
}
