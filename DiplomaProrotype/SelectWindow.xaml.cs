using DiplomaProrotype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiplomaPrototype
{
    public partial class SelectWindow : Window
    {
        static public int currentNumber;
        static public int currentPlaces;
        static public string currentWord;


        public SelectWindow()
        {
            InitializeComponent();

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = (screenHeight - this.Height) / 2 - 100;

            this.NumberTextBox.PreviewTextInput += new TextCompositionEventHandler(NumberTextBox_PreviewTextInput);
            this.PlacesTextBox.PreviewTextInput += new TextCompositionEventHandler(NumberTextBox_PreviewTextInput);
        }


        static private void SetZeroValues()
        {
            currentNumber = 0;
            currentPlaces = 0;
            currentWord = "";
        }

        static public void CreateMachineSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.NumberTextBox.Focus();
            MainWindow.selectWindow.FirstLabel.Content = "[1-5] Количество процессов:";
            MainWindow.selectWindow.SecondLabel.Visibility = Visibility.Hidden;

            MainWindow.selectWindow.PlacesTextBox.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.PlacesTextBox.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }

        static public void CreateMovableSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.NumberTextBox.Focus();
            MainWindow.selectWindow.FirstLabel.Content = "[1+] Количество техники:";
            MainWindow.selectWindow.SecondLabel.Content = "[1-3] Количество мест:";
            MainWindow.selectWindow.ShowDialog();
        }

        static public void CreateStopSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.NumberTextBox.Focus();
            MainWindow.selectWindow.FirstLabel.Content = "Технологическая цепь №:";
            MainWindow.selectWindow.SecondLabel.Content = "Вид операции на стоянке:";
            MainWindow.selectWindow.WordComboBox.Visibility = Visibility.Visible;

            MainWindow.selectWindow.PlacesTextBox.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.PlacesTextBox.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }


        void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) // Проверка на цифры
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }


        #region Кнопки
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (NumberTextBox.Text != "") currentNumber = Convert.ToInt32(NumberTextBox.Text);
            if (PlacesTextBox.Text != "") currentPlaces = Convert.ToInt32(PlacesTextBox.Text);
            currentWord = WordComboBox.Text;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
