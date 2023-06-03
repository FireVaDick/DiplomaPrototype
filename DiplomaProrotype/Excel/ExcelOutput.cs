using DiplomaProrotype;
using Haley.WPF.Controls;
using Spire.Xls;
using Spire.Xls.Core;
using System;
using System.IO;

namespace DiplomaPrototype.Excel
{
    internal class ExcelOutput
    {
        static string baseCell = "B2";

        static public void CreateExcelOutput()
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            sheet.Range["A1"].ColumnWidth = 35;


            // Самая первая общая строчка
            for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
            {
                var currentColumn = Convert.ToString(Convert.ToChar(65 + j));
                var currentCell = string.Format("{0}{1}", currentColumn, 1);

                sheet.Range[currentCell].Value = MainWindow.matrixResourceMachine[0, j];
            }
            for (int j = 0; j < MainWindow.vectorChain.Count; j++)
            {
                var currentColumn = Convert.ToString(Convert.ToChar(65 + j + MainWindow.matrixResourceMachine.GetLength(1)));
                var currentCell = string.Format("{0}{1}", currentColumn, 1);

                sheet.Range[currentCell].Value = MainWindow.matrixChainStop[0, j];
            }

            // Станки
            for (int i = 1; i < MainWindow.machineTiles.Count + 1; i++)
            {
                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1);

                    sheet.Range[currentCell].Value = MainWindow.matrixResourceMachine[i, j];
                }
            }

            // Стоянки
            for (int i = 1; i < MainWindow.stopTiles.Count + 1; i++)
            {
                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + MainWindow.matrixResourceMachine.GetLength(0));

                    sheet.Range[currentCell].Value = MainWindow.matrixResourceStop[i, j];
                }

                for (int j = 0; j < MainWindow.vectorChain.Count; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j + MainWindow.matrixResourceStop.GetLength(1)));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + MainWindow.matrixResourceMachine.GetLength(0));

                    sheet.Range[currentCell].Value = MainWindow.matrixChainStop[i, j];
                }
            }

            // Переезды
            for (int i = 1; i < MainWindow.matrixCrossings.GetLength(0); i++)
            {
                var currentCell = string.Format("{0}{1}", "A", i + MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0) - 1);

                sheet.Range[currentCell].Value = MainWindow.matrixCrossings[i, 0];
            }

            // Диспетчер имён
            INamedRange NamedRange1 = workbook.NameRanges.Add("Начало");
            NamedRange1.RefersToRange = sheet.Range["A1"];

            INamedRange NamedRange2 = workbook.NameRanges.Add("МатрСвязиОпераций");
            int endRow = MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0) - 1 + MainWindow.matrixCrossings.GetLength(0) - 1;
            var endColumn = Convert.ToString(Convert.ToChar(65 + MainWindow.resourceTiles.Count + MainWindow.vectorChain.Count));
            NamedRange2.RefersToRange = sheet.Range[string.Format("{0}:{1}{2}", baseCell, endColumn, endRow)];

            // Запись
            try
            {
                if (File.Exists(@"..\\Result.xlsx"))
                {
                    File.Delete(@"..\\Result.xlsx");
                }

                workbook.SaveToFile(@"..\\Result.xlsx", ExcelVersion.Version2013);
            }
            catch { };

            //Process.Start("excel.exe", @"..\\Result");
        }
    }
}
