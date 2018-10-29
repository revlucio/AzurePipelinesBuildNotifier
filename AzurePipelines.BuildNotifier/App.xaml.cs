using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
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

            _notifyIcon = (TaskbarIcon) FindResource("RedIcon");

            _notifyIcon2 = (TaskbarIcon) FindResource("GreenIcon");
            var greenVm = (NotifyIconViewModel) _notifyIcon2.DataContext;
            greenVm.MyVisibility = Visibility.Hidden;
            
            StartFileWatcher();

//            while (true)
//            {
//                Thread.Sleep(TimeSpan.FromSeconds(10));
//                CheckBuildStatus();
//            }
        }

        private void CheckBuildStatus()
        {
            var collectionUri = "https://dev.azure.com/revlucio";
            var token = "befbvymjfrdx6ba74xnm732s6x2gicvdcfcbanxydnbeo7p3rlra";
            var connection = new VssConnection(new Uri(collectionUri), new VssBasicCredential("build-api", token));

            var buildClient = connection.GetClient<BuildHttpClient>();

            var project = "street-runner";
            var definitions = new[] {4};

            var successful = buildClient
              .GetBuildsAsync(project, definitions).Result
              .Where(build => build.Status == BuildStatus.Completed)
              .OrderBy(build => build.StartTime)
              .Last()
              .Result == BuildResult.Succeeded;
            
            ChangeIcons(successful == false);
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
                var redVm = (NotifyIconViewModel) _notifyIcon.DataContext;
                var greenVm = (NotifyIconViewModel) _notifyIcon2.DataContext;
                
                if (red)
                {
                    redVm.MyVisibility = Visibility.Visible;
                    greenVm.MyVisibility = Visibility.Hidden;
                    _notifyIcon.ShowBalloonTip("Build Failed", "A build has failed", BalloonIcon.None);
                }
                else
                {
                    redVm.MyVisibility = Visibility.Hidden;
                    greenVm.MyVisibility = Visibility.Visible;
                    _notifyIcon2.ShowBalloonTip("Build Passed", "A build has passed", BalloonIcon.None);
                }
            });
        }
    }
}