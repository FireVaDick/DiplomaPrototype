using DataModeling.Models;
using System.Collections.ObjectModel;

namespace DataModeling.DataSource
{
    public interface IDataSource
    {
        ObservableCollection<IndustrialChain> GetData();
    }
}
