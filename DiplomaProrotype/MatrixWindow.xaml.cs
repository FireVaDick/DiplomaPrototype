using DiplomaProrotype.Animations;
using DiplomaProrotype.CanvasManipulation;
using Haley.Utils;
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
        public MatrixWindow()
        {
            InitializeComponent();

            FlowDoc = new FlowDocument();

            CreateMatrix();

            FlowDocumentReader myFlowDocumentReader = new FlowDocumentReader();
            myFlowDocumentReader.Document = FlowDoc;
            myFlowDocumentReader.ViewingMode = FlowDocumentReaderViewingMode.Scroll;

            this.Content = myFlowDocumentReader;

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            this.Left = (screenWidth - this.Width) - 65;
            this.Top = (screenHeight - this.Height) / 2 - 25;
        }


        private void CreateMatrix()
        {
            Matrix = new Table();
            Matrix.CellSpacing = 10;
            Matrix.FontFamily = new FontFamily("Cambria");

            FlowDoc.Blocks.Add(Matrix);

            Matrix.RowGroups.Add(new TableRowGroup());
            TableRow currentRow;

            // Инициализация строк
            for (int i = 0; i < MainWindow.machineTiles.Count + MainWindow.stopTiles.Count + MainWindow.matrixCrossings.GetLength(0) + 1; i++)
            {
                Matrix.RowGroups[0].Rows.Add(new TableRow());
            }

            // Визуальное оформление строк
            for (int x = 0; x < MainWindow.resourceTiles.Count + MainWindow.vectorChain.Count + 1; x++)
            {
                Matrix.Columns.Add(new TableColumn());

                if (x % 2 == 0)
                    Matrix.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)229, (byte)233, (byte)236));
                else
                    Matrix.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)132, (byte)169, (byte)140));
            }


            // Самая первая общая строчка
            for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
            {
                currentRow = Matrix.RowGroups[0].Rows[0];

                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixResourceMachine[0, j]))));

                currentRow.FontSize = 16;
                currentRow.FontWeight = FontWeights.Bold;
                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;

            }
            for (int j = 0; j < MainWindow.vectorChain.Count; j++)
            {
                currentRow = Matrix.RowGroups[0].Rows[0];

                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixChainStop[0, j]))));

            }

            // Станки
            for (int i = 1; i < MainWindow.machineTiles.Count + 1; i++)
            {
                Matrix.RowGroups[0].Rows.Add(new TableRow());
                currentRow = Matrix.RowGroups[0].Rows[i];

                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixResourceMachine[i, j]))));

                    currentRow.Cells[0].FontSize = 16;
                    currentRow.Cells[0].FontWeight = FontWeights.Bold;
                }
            }

            // Стоянки
            for (int i = 1; i < MainWindow.stopTiles.Count + 1; i++)
            {
                Matrix.RowGroups[0].Rows.Add(new TableRow());
                currentRow = Matrix.RowGroups[0].Rows[i + MainWindow.matrixResourceMachine.GetLength(0)];

                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixResourceStop[i, j]))));

                    currentRow.Cells[0].FontSize = 16;
                    currentRow.Cells[0].FontWeight = FontWeights.Bold;
                }

                for (int j = 0; j < MainWindow.vectorChain.Count; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixChainStop[i, j]))));

                    currentRow.Cells[0].FontSize = 16;
                    currentRow.Cells[0].FontWeight = FontWeights.Bold;
                }
            }

            // Переезды
            for (int i = 1; i < MainWindow.matrixCrossings.GetLength(0); i++)
            {
                Matrix.RowGroups[0].Rows.Add(new TableRow());
                currentRow = Matrix.RowGroups[0].Rows[i + MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0)];

                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(MainWindow.matrixCrossings[i]))));

                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
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
