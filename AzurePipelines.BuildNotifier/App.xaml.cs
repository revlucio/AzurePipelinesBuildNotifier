using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Windowless_Sample;

namespace AzurePipelines.BuildNotifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon _notifyIcon;
        private TaskbarIcon _notifyIcon2;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            _notifyIcon2 = (TaskbarIcon) FindResource("GreenIcon");
            var greenVm = (NotifyIconViewModel) _notifyIcon2.DataContext;
            greenVm.MyVisibility = Visibility.Hidden;

            var watcher = new FileSystemWatcher();
            
            watcher.Path = "C:\\temp\\watcher";
            watcher.Filter = "build.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                var redVm = (NotifyIconViewModel) _notifyIcon.DataContext;
                var greenVm = (NotifyIconViewModel) _notifyIcon2.DataContext;

                try
                {
                    var content = File.ReadAllText(e.FullPath);
                    if (content.Contains("red"))
                    {
                        redVm.MyVisibility = Visibility.Visible;
                        greenVm.MyVisibility = Visibility.Hidden;    
                    }
                    else
                    {
                        redVm.MyVisibility = Visibility.Hidden;
                        greenVm.MyVisibility = Visibility.Visible;
                    }
                }
                catch (IOException)
                {
                    
                }
                
            });
        }
    }
}