using System;

namespace Charts.DataSource
{
    interface IDataSource
    {
        IObservable<object> GetData();
    }
}
