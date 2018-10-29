using System.IO;
using Newtonsoft.Json.Linq;

namespace AzurePipelines.BuildNotifier
{
    internal class AzureSettings
    {
        private readonly JObject _json;
        private static readonly string SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "azure.settings");

        private AzureSettings(JObject json)
        {
            _json = json;
        }

        public string Token => _json.Value<string>("token");
        public string ServerUrl => _json.Value<string>("serverUrl");
        public string ProjectName => _json.Value<string>("projectName");
        public int BuildId => _json.Value<int>("buildId");

        public static AzureSettings FromFileSystem()
        {
            if (!File.Exists(SettingsPath))
            {
                Save(string.Empty, string.Empty, string.Empty, string.Empty);
            }
                
            return new AzureSettings(JObject.Parse(File.ReadAllText(SettingsPath)));
        }
        
        public static void Save(string token, string serverUrl, string projectName, string buildId)
        {
            var settings = JObject.FromObject(new
            {
                token,
                serverUrl,
                projectName,
                buildId,
            });

            File.WriteAllText(SettingsPath, settings.ToString());
        }
    }
}