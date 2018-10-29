using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.TeamFoundation.Build.WebApi;
using Windowless_Sample;

namespace AzurePipelines.BuildNotifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon _redIcon;
        private TaskbarIcon _greenIcon;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _redIcon = (TaskbarIcon) FindResource("RedIcon");

            _greenIcon = (TaskbarIcon) FindResource("GreenIcon");
            var greenVm = (NotifyIconViewModel) _greenIcon.DataContext;
            greenVm.MyVisibility = Visibility.Hidden;
            
//            StartFileWatcher();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    CheckBuildStatus2();
                }    
            });
        }

        public void CheckBuildStatus()
        {
            var buildServer = new BuildServer(
                "https://dev.azure.com/revlucio", 
                "befbvymjfrdx6ba74xnm732s6x2gicvdcfcbanxydnbeo7p3rlra");

            ChangeIcons(buildServer.WasLastBuildSuccessful("street-runner", 4) == false);
        }
        
        public void CheckBuildStatus2()
        {
            var azureSettings = AzureSettings.FromFileSystem();
            
            var buildServer = new BuildServer(
                azureSettings.ServerUrl, 
                azureSettings.Token);

            ChangeIcons(buildServer.WasLastBuildSuccessful(azureSettings.ProjectName, azureSettings.BuildId) == false);
        }

        private void StartFileWatcher()
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = "C:\\temp\\watcher";
            watcher.Filter = "build.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                var content = File.ReadAllText(e.FullPath);
                var red = content.Contains("red");

                ChangeIcons(red);
            }
            catch (IOException)
            {

            }
        }

        private void ChangeIcons(bool red)
        {
            this.Dispatcher.Invoke(() =>
            {
                var redVm = (NotifyIconViewModel) _redIcon.DataContext;
                var greenVm = (NotifyIconViewModel) _greenIcon.DataContext;
                
                if (red)
                {
                    redVm.MyVisibility = Visibility.Visible;
                    greenVm.MyVisibility = Visibility.Hidden;
                    _redIcon.ShowBalloonTip("Build Failed", "A build has failed", BalloonIcon.None);
                }
                else
                {
                    redVm.MyVisibility = Visibility.Hidden;
                    greenVm.MyVisibility = Visibility.Visible;
                    _greenIcon.ShowBalloonTip("Build Passed", "A build has passed", BalloonIcon.None);
                }
            });
        }
    }
}