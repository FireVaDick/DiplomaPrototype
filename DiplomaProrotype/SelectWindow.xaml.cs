﻿using DiplomaProrotype;
using System;
using System.Windows;
using System.Windows.Input;

namespace DiplomaPrototype
{
    public partial class SelectWindow : Window
    {
        static public int currentNumber1;
        static public int currentNumber2;
        static public int currentNumber3;
        static public string currentWord = "";
        static public string currentText = "";


        public SelectWindow()
        {
            InitializeComponent();

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = (screenHeight - this.Height) / 2 - 100;

            this.TextBox1.PreviewTextInput += new TextCompositionEventHandler(NumberTextBox_PreviewTextInput);
            this.TextBox2.PreviewTextInput += new TextCompositionEventHandler(NumberTextBox_PreviewTextInput);
            this.TextBox3.PreviewTextInput += new TextCompositionEventHandler(NumberTextBox_PreviewTextInput);
        }


        static private void SetZeroValues()
        {
            currentNumber1 = 0;
            currentNumber2 = 0;
            currentNumber3 = 0;
            currentWord = "";
            currentText = "";
        }

        static public void CreateResourceSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.TextBox4.Focus();
            MainWindow.selectWindow.Label1.Content = "Имя задела:";
            MainWindow.selectWindow.Label2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.Label3.Visibility = Visibility.Hidden;
        
            MainWindow.selectWindow.TextBox4.Visibility = Visibility.Visible;
            MainWindow.selectWindow.TextBox4.IsEnabled = true;

            MainWindow.selectWindow.TextBox1.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox1.IsEnabled = false;
            MainWindow.selectWindow.TextBox2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox2.IsEnabled = false;
            MainWindow.selectWindow.TextBox3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox3.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }

        static public void CreateMachineSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.TextBox1.Focus();
            MainWindow.selectWindow.Label1.Content = "[1-5] Количество процессов:";
            MainWindow.selectWindow.Label2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.Label3.Visibility = Visibility.Hidden;

            MainWindow.selectWindow.TextBox2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox2.IsEnabled = false;
            MainWindow.selectWindow.TextBox3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox3.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }

        static public void CreateMovableSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.TextBox1.Focus();
            MainWindow.selectWindow.Label1.Content = "[1+] Количество техники:";
            MainWindow.selectWindow.Label2.Content = "[1-3] Количество мест:";
            MainWindow.selectWindow.Label3.Visibility = Visibility.Hidden;

            MainWindow.selectWindow.TextBox3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox3.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }

        static public void CreateStopSelectWindow()
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.TextBox1.Focus();
            MainWindow.selectWindow.Label1.Content = "Технологическая цепь №:";
            MainWindow.selectWindow.Label2.Content = "Вид операции на стоянке:";
            MainWindow.selectWindow.Label3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.WordComboBox.Visibility = Visibility.Visible;

            MainWindow.selectWindow.TextBox2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox2.IsEnabled = false;
            MainWindow.selectWindow.TextBox3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox3.IsEnabled = false;
            MainWindow.selectWindow.ShowDialog();
        }

        static public void AddMovableToMatrixParticipation(int places)
        {
            SetZeroValues();

            MainWindow.selectWindow = new SelectWindow();
            MainWindow.selectWindow.TextBox1.Focus();
            MainWindow.selectWindow.Label1.Content = "Первое место участвует в цепи:";
            MainWindow.selectWindow.Label2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.Label3.Visibility = Visibility.Hidden;

            MainWindow.selectWindow.TextBox2.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox2.IsEnabled = false;
            MainWindow.selectWindow.TextBox3.Visibility = Visibility.Hidden;
            MainWindow.selectWindow.TextBox3.IsEnabled = false;

            if (places > 1)
            {
                MainWindow.selectWindow.Label2.Visibility = Visibility.Visible;
                MainWindow.selectWindow.Label2.Content = "Второе место участвует в цепи: ";               
                MainWindow.selectWindow.TextBox2.Visibility = Visibility.Visible;
                MainWindow.selectWindow.TextBox2.IsEnabled = true;
            }
            if (places > 2)
            {
                MainWindow.selectWindow.Label3.Visibility = Visibility.Visible;
                MainWindow.selectWindow.Label3.Content = "Третье место участвует в цепи: ";
                MainWindow.selectWindow.TextBox3.Visibility = Visibility.Visible;
                MainWindow.selectWindow.TextBox3.IsEnabled = true;
            }

            MainWindow.selectWindow.ShowDialog();
        }


        void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) // Проверка на цифры
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }


        #region Кнопки
        private void SelectWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (TextBox1.Text != "") currentNumber1 = Convert.ToInt32(TextBox1.Text);
                    if (TextBox2.Text != "") currentNumber2 = Convert.ToInt32(TextBox2.Text);
                    if (TextBox3.Text != "") currentNumber3 = Convert.ToInt32(TextBox3.Text);
                    currentWord = WordComboBox.Text;
                    currentText = TextBox4.Text;
                    this.Close();
                    break;
            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text != "") currentNumber1 = Convert.ToInt32(TextBox1.Text);
            if (TextBox2.Text != "") currentNumber2 = Convert.ToInt32(TextBox2.Text);
            if (TextBox3.Text != "") currentNumber3 = Convert.ToInt32(TextBox3.Text);
            currentWord = WordComboBox.Text;
            currentText = TextBox4.Text;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
