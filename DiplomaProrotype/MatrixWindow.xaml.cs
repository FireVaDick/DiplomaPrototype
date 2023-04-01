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


        public MatrixWindow()
        {
            InitializeComponent();

            CreateMatrixTable();

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            this.Left = (screenWidth - this.Width) - 15;
            this.Top = (screenHeight - this.Height) / 2 - 30;
        }

        private void CreateMatrixTable()
        {
            FlowDoc = new FlowDocument();

            MatrixTable = new Table();
            MatrixTable.CellSpacing = 10;
            MatrixTable.FontFamily = new FontFamily("Cambria");

            FlowDoc.Blocks.Add(MatrixTable);

            // Визуальное оформление колонок
            for (int x = 0; x < resourceTiles.Count + 1; x++)
            {
                MatrixTable.Columns.Add(new TableColumn());
                
                if (x % 2 == 0)
                    MatrixTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)229, (byte)233, (byte)236));
                else
                    MatrixTable.Columns[x].Background = new SolidColorBrush(Color.FromArgb(255, (byte)132, (byte)169, (byte)140));
            }

            MatrixTable.RowGroups.Add(new TableRowGroup());
            MatrixTable.RowGroups[0].Rows.Add(new TableRow());

            TableRow currentRow = MatrixTable.RowGroups[0].Rows[0];

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Bold;

            // Оформление заголовков
            for (int j = 0; j < resourceTiles.Count; j++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(j + 1)))));
            }

            for (int i = 0; i < machineTiles.Count; i++)
            {

                MatrixTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = MatrixTable.RowGroups[0].Rows[i + 1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(i + 1)))));

                currentRow.Cells[0].FontSize = 16;
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
            }

            // Данные самой матрицы
            for (int i = 0; i < machineTiles.Count; i++)
            {                           
                currentRow = MatrixTable.RowGroups[0].Rows[i + 1];

                for (int j = 0; j < resourceTiles.Count; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(MainWindow.matrixResourceMachine[i, j])))));
                }
            }


            FlowDocumentReader myFlowDocumentReader = new FlowDocumentReader();
            myFlowDocumentReader.Document = FlowDoc;

            this.Content = myFlowDocumentReader;
        }     
    }
}
