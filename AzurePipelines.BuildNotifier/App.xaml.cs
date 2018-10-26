using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace AzurePipelines.BuildNotifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon _notifyIcon;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");
        }
    }
}