using DataModeling.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace DataModeling.DataSource
{
    public class ExcelDataSource : IDataSource
    {
        private const string FileNotFound = "\n[ERROR]: Excel-файл с исходными данными не найден, проверьте указанный путь.\n";

        private readonly string filePath;

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
        }

        public ObservableCollection<IndustrialChain> GetData()
        {
            //return new ObservableCollection<IndustrialChain>();

            throw new NotImplementedException();
        }
    }
}
