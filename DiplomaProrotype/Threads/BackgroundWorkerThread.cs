using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DiplomaProrotype.Threads
{
    internal class BackgroundWorkerThread
    {
        static public void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; ++i)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        static public void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MainWindow.machineTileFromContextMenu.MachineProgress1.Value = e.ProgressPercentage;
            MainWindow.machineTileFromContextMenu.MachineProgress2.Value = e.ProgressPercentage;
            MainWindow.machineTileFromContextMenu.MachineProgress3.Value = e.ProgressPercentage;
            MainWindow.machineTileFromContextMenu.MachineProgress4.Value = e.ProgressPercentage;
            MainWindow.machineTileFromContextMenu.MachineProgress5.Value = e.ProgressPercentage;
        }

        static public void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MainWindow.machineTileFromContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#DC143C");
        }
    }
}
