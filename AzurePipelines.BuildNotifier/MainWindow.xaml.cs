using System;
using System.Windows;

namespace AzurePipelines.BuildNotifier
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var azureSettings = AzureSettings.FromFileSystem();
            Token.Text = azureSettings.Token;
            ServerUrl.Text = azureSettings.ServerUrl;
            ProjectName.Text = azureSettings.ProjectName;
            BuildId.Text = azureSettings.BuildId.ToString();

            SaveButton.Click += Saved;
        }

        private void Saved(object sender, RoutedEventArgs e)
        {
            try
            {
                AzureSettings.Save(Token.Text, ServerUrl.Text, ProjectName.Text, BuildId.Text);

                Message.Text = "details have been saved";
            }
            catch (Exception ex)
            {
                Message.Text = ex.ToString();
            }
            
        }

        
    }
}