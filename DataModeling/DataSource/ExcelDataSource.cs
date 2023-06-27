using Bytescout.Spreadsheet;
using DataModeling.Creators;
using DataModeling.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace DataModeling.DataSource
{
    public class ExcelDataSource : IDataSource
    {
        private const string StartChainsCell = "A2";
        private const string StartOperationsCell = "A2";
        private const string StartTriadsCell = "A5";

        private const string FileNotFound = "Excel-файл с исходными данными не найден, проверьте указанный путь.";

        private readonly string filePath;

        private readonly Spreadsheet document;

        private readonly ISeriesCreator seriesCreator;

        public ExcelDataSource(string filePath)
        {
            #region Exceptions

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(FileNotFound, nameof(filePath));
            }

            #endregion

            this.filePath = filePath;
        
            document = new Spreadsheet();
            document.LoadFromFile(filePath);

            seriesCreator = new SeriesCreator();
        }

        public ObservableCollection<IndustrialChain> GetData()
        {
            var chains = new ObservableCollection<IndustrialChain>();

            return chains;
        }

        private ObservableCollection<IndustrialChain> ParseChains()
        {
            var items = new ObservableCollection<IndustrialChain>();
            var workSheet = document.Workbook.Worksheets.ByName("Цепочки");
            var cell = workSheet[StartChainsCell];
            var column = cell.ColIndex;
            var row = cell.RowIndex;

            for (var i = row; cell.Value != null; i++, cell = workSheet.Cell(i, column))
            {
                var number = (uint)cell.ValueAsInteger;
                var name = workSheet.Cell(i, column + 1).ValueAsString;

                //items.Add(new IndustrialChain(number, name,))
            }

            return new ObservableCollection<IndustrialChain>() { };
        }

        private void ParseTriads()
        {

        }

        private void ParseOperations()
        {

        }
    }
}
