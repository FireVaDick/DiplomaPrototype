using DiplomaProrotype;
using DiplomaProrotype.ObjectsManipulation;
using Spire.Xls;
using Spire.Xls.Core;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DiplomaPrototype.Excel
{
    internal class ExcelOutput
    {
        static int baseCellNumber = 66;
        static string baseMatrixCell = "C2";

        static public void CreateExcelOutput()
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            sheet.Range["A1"].ColumnWidth = 2;
            sheet.Range["B1"].ColumnWidth = 33;
            sheet.Range["P1"].ColumnWidth = 27;

            sheet.Range["Q1"].ColumnWidth = 12;
            sheet.Range["R1"].ColumnWidth = 12;
            sheet.Range["S1"].ColumnWidth = 12;
            sheet.Range["T1"].ColumnWidth = 12;
            sheet.Range["U1"].ColumnWidth = 12;

            // Самая первая общая строчка
            for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
            {
                var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber + j));
                var currentCell = string.Format("{0}{1}", currentColumn, 1);

                sheet.Range[currentCell].Value = MainWindow.matrixResourceMachine[0, j];
            }
            for (int j = 0; j < MainWindow.vectorChain.Count; j++)
            {
                var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber + j + MainWindow.matrixResourceMachine.GetLength(1)));
                var currentCell = string.Format("{0}{1}", currentColumn, 1);

                sheet.Range[currentCell].Value = MainWindow.matrixChainStop[0, j];
            }

            // Номера операций
            for (int i = 1; i < MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0) + MainWindow.matrixCrossings.GetLength(0) - 2; i++)
            {
                var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber - 1));
                var currentCell = string.Format("{0}{1}", currentColumn, i + 1);

                sheet.Range[currentCell].Value = Convert.ToString(i);
            }

            // Станки
            for (int i = 1; i < MainWindow.machineTiles.Count + 1; i++)
            {
                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1);

                    sheet.Range[currentCell].Value = MainWindow.matrixResourceMachine[i, j];
                }
            }

            // Стоянки
            for (int i = 1; i < MainWindow.stopTiles.Count + 1; i++)
            {
                for (int j = 0; j < MainWindow.resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + MainWindow.matrixResourceMachine.GetLength(0));

                    sheet.Range[currentCell].Value = MainWindow.matrixResourceStop[i, j];
                }

                for (int j = 0; j < MainWindow.vectorChain.Count; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(baseCellNumber + j + MainWindow.matrixResourceStop.GetLength(1)));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + MainWindow.matrixResourceMachine.GetLength(0));

                    try { sheet.Range[currentCell].Value = MainWindow.matrixChainStop[i, j]; }
                    catch { }
                }
            }

            // Переезды
            for (int i = 1; i < MainWindow.matrixCrossings.GetLength(0); i++)
            {
                var currentCell = string.Format("{0}{1}", "B", i + MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0) - 1);

                sheet.Range[currentCell].Value = MainWindow.matrixCrossings[i];
            }

            // Диспетчер имён
            INamedRange NamedRange1 = workbook.NameRanges.Add("Начало");
            NamedRange1.RefersToRange = sheet.Range["B1"];

            INamedRange NamedRange2 = workbook.NameRanges.Add("МатрСвязиОпераций");
            int endRow = MainWindow.matrixResourceMachine.GetLength(0) + MainWindow.matrixResourceStop.GetLength(0) - 1 + MainWindow.matrixCrossings.GetLength(0) - 1;
            var endColumn = Convert.ToString(Convert.ToChar(baseCellNumber + MainWindow.resourceTiles.Count + MainWindow.vectorChain.Count));
            NamedRange2.RefersToRange = sheet.Range[string.Format("{0}:{1}{2}", baseMatrixCell, endColumn, endRow)];


            // -----------Параметры транспорта
            MainWindow.transportParameters = ObjectPlacement.ResizeArray(MainWindow.transportParameters, MainWindow.stopTiles.Count + 3 + MainWindow.amountUnloading, MainWindow.stopTiles.Count + 2);

            // Столбцы
            for (int j = 1; j < MainWindow.stopTiles.Count + 1; j++)
            {
                MainWindow.transportParameters[0, j] = MainWindow.stopTiles[j - 1].Text + " " + MainWindow.stopTiles[j - 1].Id;
            }
            MainWindow.transportParameters[0, MainWindow.stopTiles.Count + 1] = "Фиктивная";

            // Строки
            for (int i = 1; i < MainWindow.stopTiles.Count + 1; i++)
            {
                MainWindow.transportParameters[i, 0] = "Переезд с [" + MainWindow.stopTiles[i - 1].Text + " " + MainWindow.stopTiles[i - 1].Id + "]";
            }
            MainWindow.transportParameters[MainWindow.stopTiles.Count + 1, 0] = "Фиктивная";
            MainWindow.transportParameters[MainWindow.stopTiles.Count + 2, 0] = "Переезд с [Начальная]";

            for (int i = MainWindow.stopTiles.Count + 3, counter = 1; i < MainWindow.transportParameters.GetLength(0); i++, counter++)
            {
                MainWindow.transportParameters[i, 0] = "Переезд с [Промежуточная " + counter + "]";
            }

            // Заполнение
            int transportOperationNumber = MainWindow.operations.Count;

            for (int i = 1; i < MainWindow.matrixCrossings.GetLength(0); i++)
            {
                Regex rowRegex = new Regex(@"\[(\w+) \d");
                Regex columnRegex = new Regex(@"(\w+) \d\]");
                MatchCollection rowCrossings = rowRegex.Matches(MainWindow.matrixCrossings[i]);
                MatchCollection columnCrossings = columnRegex.Matches(MainWindow.matrixCrossings[i]);

                bool isFounded = false;

                for (int k = 1; k < MainWindow.transportParameters.GetLength(0); k++)
                {
                    MatchCollection rowTransport = rowRegex.Matches(MainWindow.transportParameters[k, 0]);

                    if (rowCrossings.Count == 1 && rowTransport.Count == 1 && rowCrossings[0].Value == rowTransport[0].Value)
                    {
                        for (int j = 1; j < MainWindow.transportParameters.GetLength(1); j++)
                        {
                            if (columnCrossings.Count == 1 && Convert.ToString(columnCrossings[0].Value) == string.Format("{0}]", MainWindow.transportParameters[0, j]))
                            {
                                MainWindow.transportParameters[k, j] = Convert.ToString(++transportOperationNumber);
                                isFounded = true;
                                break;
                            }
                        }
                    }

                    if (isFounded) break;
                }
            }






/*            for (int i = 0; i < MainWindow.transportParameters.GetLength(0); i++)
            {
                for (int j = 0; j < MainWindow.transportParameters.GetLength(1); j++)
                {
                    

                }
            }*/





            // Вывод параметров
            for (int i = 0; i < MainWindow.transportParameters.GetLength(0); i++)
            {
                for (int j = 0; j < MainWindow.transportParameters.GetLength(1); j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(80 + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1);

                    sheet.Range[currentCell].Value = MainWindow.transportParameters[i, j];
                }
            }


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
