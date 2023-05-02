﻿using DiplomaProrotype.Animations;
using DiplomaProrotype.CanvasManipulation;
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
using System.Windows.Shapes;

namespace DiplomaProrotype
{
    /// <summary>
    /// Логика взаимодействия для MatrixWindow.xaml
    /// </summary>
    public partial class MatrixWindow : Window
    {
        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;


        public MatrixWindow()
        {
            InitializeComponent();

            FlowDoc = new FlowDocument();

            if (resourceTiles.Count != 0 && machineTiles.Count != 0)
            {
                CreateMatrixResourceMachine();
            }

            if (resourceTiles.Count != 0 && stopTiles.Count != 0)
            {
                CreateMatrixResourcePlaceStop();
            }

            FlowDocumentReader myFlowDocumentReader = new FlowDocumentReader();
            myFlowDocumentReader.Document = FlowDoc;

            this.Content = myFlowDocumentReader;

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            this.Left = (screenWidth - this.Width) - 65;
            this.Top = (screenHeight - this.Height) / 2 - 30;
        }


        private void CreateMatrixResourceMachine()
        {
            MatrixResourceMachineTable = new Table();
            MatrixResourceMachineTable.CellSpacing = 10;
            MatrixResourceMachineTable.FontFamily = new FontFamily("Cambria");

            FlowDoc.Blocks.Add(MatrixResourceMachineTable);

            // Визуальное оформление колонок
            for (int x = 0; x < resourceTiles.Count + 1; x++)
            {
                MatrixResourceMachineTable.Columns.Add(new TableColumn());
                
                if (x % 2 == 0)
                    MatrixResourceMachineTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)229, (byte)233, (byte)236));
                else
                    MatrixResourceMachineTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)132, (byte)169, (byte)140));
            }

            MatrixResourceMachineTable.RowGroups.Add(new TableRowGroup());
            MatrixResourceMachineTable.RowGroups[0].Rows.Add(new TableRow());

            TableRow currentRow = MatrixResourceMachineTable.RowGroups[0].Rows[0];

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Bold;

            // Оформление заголовков
            for (int j = 0; j < resourceTiles.Count; j++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Задел " + Convert.ToString(j + 1)))));
            }

            for (int i = 0; i < machineTiles.Count; i++)
            {
                MatrixResourceMachineTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = MatrixResourceMachineTable.RowGroups[0].Rows[i + 1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Станок " + Convert.ToString(i + 1)))));
                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
            }

            // Данные самой матрицы
            for (int i = 0; i < machineTiles.Count; i++)
            {                           
                currentRow = MatrixResourceMachineTable.RowGroups[0].Rows[i + 1]; // Вторая строка

                for (int j = 0; j < resourceTiles.Count; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(MainWindow.matrixResourceMachine[i, j])))));
                }
            }
        }

        private void CreateMatrixResourcePlaceStop()
        {
            MatrixResourcePlaceStopTable = new Table();
            MatrixResourcePlaceStopTable.CellSpacing = 10;
            MatrixResourcePlaceStopTable.FontFamily = new FontFamily("Cambria");

            FlowDoc.Blocks.Add(MatrixResourcePlaceStopTable);

            // Визуальное оформление колонок
            for (int x = 0; x < resourceTiles.Count + 1 + MainWindow.amountPlaces; x++)
            {
                MatrixResourcePlaceStopTable.Columns.Add(new TableColumn());

                if (x % 2 == 0)
                    MatrixResourcePlaceStopTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)229, (byte)233, (byte)236));
                else
                    MatrixResourcePlaceStopTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)132, (byte)169, (byte)140));
            }

            MatrixResourcePlaceStopTable.RowGroups.Add(new TableRowGroup());
            MatrixResourcePlaceStopTable.RowGroups[0].Rows.Add(new TableRow());

            TableRow currentRow = MatrixResourcePlaceStopTable.RowGroups[0].Rows[0];

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Bold;

            // Оформление заголовков
            for (int j = 0; j < resourceTiles.Count; j++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Задел " + Convert.ToString(j + 1)))));
            }

            for (int j = 0; j < MainWindow.amountPlaces; j++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Место " + Convert.ToString(j + 1)))));
            }

            for (int i = 0; i < MainWindow.amountLoading; i++)
            {
                MatrixResourcePlaceStopTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = MatrixResourcePlaceStopTable.RowGroups[0].Rows[i + 1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Погрузка " + Convert.ToString(i + 1)))));
                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
            }

            for (int i = 0; i < MainWindow.amountUnloading; i++)
            {
                MatrixResourcePlaceStopTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = MatrixResourcePlaceStopTable.RowGroups[0].Rows[i + 1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Разгрузка " + Convert.ToString(i + 1)))));
                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
            }

            // Данные самой матрицы
            for (int i = 0; i < MainWindow.amountLoading; i++)
            {
                currentRow = MatrixResourcePlaceStopTable.RowGroups[0].Rows[i + 1]; // Вторая строка

                for (int j = 0; j < resourceTiles.Count + MainWindow.amountPlaces; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(MainWindow.matrixResourcePlaceLoading[i, j])))));
                }
            }

            for (int i = 0; i < MainWindow.amountUnloading; i++)
            {
                currentRow = MatrixResourcePlaceStopTable.RowGroups[0].Rows[i + 1]; // Вторая строка

                for (int j = 0; j < resourceTiles.Count + MainWindow.amountPlaces; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(MainWindow.matrixResourcePlaceUnloading[i, j])))));
                }
            }
        }

        static public void CreateMatrixWindow()
        {
            if (Application.Current.Windows.OfType<MatrixWindow>().Count() > 0)
            {
                MainWindow.matrixWindow.Close();
            }

            MainWindow.matrixWindow = new MatrixWindow();
            MainWindow.matrixWindow.Show();
        }


        #region Горячие клавиши
        private async void MatrixWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close(); break;

                case Key.Tab:
                    MatrixWindow matrixWindow = new MatrixWindow();
                    matrixWindow.Show();

                    await Task.Delay(300);
                    this.Close(); 
                    break;
            }
        }
        #endregion
    }
}
