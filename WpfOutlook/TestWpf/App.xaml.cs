using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace TestWpf
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
